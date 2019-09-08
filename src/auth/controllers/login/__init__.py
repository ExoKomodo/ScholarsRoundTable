import flask
import flask_login
import json
import requests

from auth import app

from auth.helpers.user import User

import oauthlib.oauth2 as oauth2

GOOGLE_CLIENT_ID = app.config['oauth']['GOOGLE_CLIENT_ID']
GOOGLE_CLIENT_SECRET = app.config['oauth']['GOOGLE_CLIENT_SECRET']
GOOGLE_DISCOVERY_URL = app.config['oauth']['GOOGLE_DISCOVERY_URL']

login_manager = flask_login.LoginManager()
login_manager.init_app(app)

BASE_URL = 'https://localhost:5000'
if app.env == 'production':
    BASE_URL = 'https://scholarsroundtable.com'

client = oauth2.WebApplicationClient(GOOGLE_CLIENT_ID)

def __get_google_provider_config():
    return requests.get(GOOGLE_DISCOVERY_URL).json()

def __get_redirect_url():
    return flask.url_for('catch_all') if app.env == 'production' else 'https://localhost:8080'

@login_manager.user_loader
def load_user(user_id):
    return User.get(user_id)

@app.route('/auth/login')
def login():
    google_provider_config = __get_google_provider_config()
    authorization_endpoint = google_provider_config['authorization_endpoint']

    base_url = flask.request.base_url
    base_url.replace('https://localhost:5000', BASE_URL)
    request_uri = client.prepare_request_uri(
        authorization_endpoint,
        redirect_uri='{}/callback'.format(base_url),
        scope=[
            'openid',
            'email',
            'profile',
        ],
    )
    return flask.redirect(request_uri)

@app.route('/auth/login/callback')
def login_callback():
    code = flask.request.args.get('code', None)
    if not code:
        flask.abort(500, 'Got no code on callback')
    google_provider_config = __get_google_provider_config()
    token_endpoint = google_provider_config['token_endpoint']

    base_url = flask.request.base_url
    url = flask.request.url
    base_url.replace('https://localhost:5000', BASE_URL)
    url.replace('https://localhost:5000', BASE_URL)
    token_url, headers, body = client.prepare_token_request(
        token_endpoint,
        authorization_response=url,
        redirect_url=base_url,
        code=code,
    )
    token_response = requests.post(
        token_url,
        headers=headers,
        data=body,
        auth=(
            GOOGLE_CLIENT_ID,
            GOOGLE_CLIENT_SECRET,
        ),
    )

    client.parse_request_body_response(
        json.dumps(
            token_response.json()
        )
    )

    userinfo_endpoint = google_provider_config['userinfo_endpoint']
    uri, headers, body = client.add_token(userinfo_endpoint)
    userinfo_response = requests.get(
        uri,
        headers=headers,
        data=body,
    )

    if userinfo_response.json().get('email_verified'):
        user_id = userinfo_response.json()['sub']
        user_email = userinfo_response.json()['email']
        picture = userinfo_response.json()['picture']
        users_name = userinfo_response.json()['given_name']

        user = User(
            user_id=user_id,
            email=user_email,
            is_admin=False,
            name=users_name,
            profile_pic=picture,
        )

        if not User.get(user_id):
            User.create(
                user_id=user_id,
                email=user_email,
                is_admin=False,
                name=users_name,
                profile_pic=picture,
            )
        flask_login.login_user(user)
        redirect_url = '{}?userId={}'.format(
            __get_redirect_url(),
            user_id,
        )
        return flask.redirect(redirect_url)
    else:
        flask.abort(400, 'User email not available or not verified by Google.')
    
@app.route('/auth/logout')
def logout():
    flask_login.logout_user()
    return flask.redirect(__get_redirect_url())