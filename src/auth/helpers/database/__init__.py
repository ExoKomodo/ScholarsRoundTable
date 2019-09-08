import json
import pymysql

from contextlib import contextmanager
from DBUtils.SteadyDB import connect

__DB_CONFIG = {}
with open('config.json', mode='r') as __config_file:
    config = json.load(__config_file)
    __DB_CONFIG = config['database']
    # __DB_CONFIG = config['mockDatabase']

def perform_query(query, cursor=pymysql.cursors.DictCursor, get_insert_id=False, has_result=True):
    with get_cursor() as cursor:
        result = cursor.execute(query)
        if has_result or get_insert_id:
            if get_insert_id:
                result = cursor.execute('SELECT LAST_INSERT_ID()')
                result = cursor.fetchall()
                result = result[0]['LAST_INSERT_ID()']
            else:
                result = cursor.fetchall()
        return result

@contextmanager
def get_cursor(cursor_cls=pymysql.cursors.DictCursor, commit=True):
    with get_db(cursor_cls=cursor_cls) as db:
        cursor = db.cursor()
        try:
            yield cursor
        finally:
            if commit:
                db.commit()
            cursor.close()

@contextmanager
def get_db(cursor_cls=pymysql.cursors.DictCursor):
    db = connect(
        autocommit=True,
        creator=pymysql,
        cursorclass=cursor_cls,
        db=__DB_CONFIG['name'],
        host=__DB_CONFIG['host'],
        passwd=__DB_CONFIG['password'],
        port=__DB_CONFIG['port'],
        user=__DB_CONFIG['user'],
    )
    try:
        yield db
    finally:
        db.close()
