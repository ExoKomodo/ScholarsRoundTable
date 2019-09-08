from flask_login import UserMixin

class User(UserMixin):
    def __init__(
        self,
        user_id,
        email,
        is_admin,
        name,
        profile_pic,
    ):
        self.id = user_id
        self.email = email
        self.is_admin = is_admin
        self.name = name
        self.profile_pic = profile_pic

    @staticmethod
    def get(user_id):
        from auth.helpers import database

        with database.get_cursor() as cursor:
            cursor.execute(
                'SELECT * FROM Users WHERE id = %s',
                (
                    user_id,
                )
            )
            user = cursor.fetchone()
            if not user:
                return None

            user = User(
                user_id=user['id'],
                email=user['email'],
                is_admin=not not user['is_admin'],
                name=user['name'],
                profile_pic=user['profile_pic'],
            )
            return user

    @staticmethod
    def create(
        user_id,
        email,
        is_admin,
        name,
        profile_pic,
    ):
        from auth.helpers import database

        with database.get_cursor(commit=True) as cursor:
            cursor.execute(
                'INSERT INTO Users (id, email, is_admin, name, profile_pic) VALUES (%s, %s, %s, %s, %s)',
                (
                    user_id,
                    email,
                    is_admin,
                    name,
                    profile_pic,
                ),
            )
