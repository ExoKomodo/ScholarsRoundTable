import flask
import flask_login
import json

from auth.helpers.user import User

app = flask.Flask(
    __name__,
    static_folder = './dist/static',
    template_folder = './dist',
)

with open('config.json', mode='r') as __config_file:
    config = json.load(__config_file)
    for key, value in config.items():
        app.config[key] = value

app.secret_key = app.config['SECRET_KEY']

import auth.controllers.login

@app.before_request
def before_request():
    flask.session['is_logged_in'] = flask_login.current_user.is_authenticated

@app.after_request
def after_request(response):
    response.headers.add('Access-Control-Allow-Origin', '*')
    response.headers.add('Access-Control-Allow-Methods', 'GET, POST, PUT, , DELETE, OPTIONS')
    response.headers.add('Access-Control-Allow-Headers', 'Content-Type,Authorization')
    response.headers.add('Access-Control-Expose-Headers', 'Content-Type,Content-Length,Authorization,X-Pagination')

    return response

@app.route('/auth/user-info')
def get_user_info():
    user_info = {}
    user_id = flask.request.args.get('userId', None)
    if not user_id:
        flask.abort(400, 'Query params must contain user id')
    user = User.get(user_id)
    user_info['profilePic'] = user.profile_pic
    user_info['isAdmin'] = user.is_admin
    user_info['email'] = user.email
    user_info['name'] = user.name
    user_info['id'] = user.id
    return user_info

@app.route('/', defaults={'path': ''})
@app.route('/<path:path>')
def catch_all(path):
    return '', 200
