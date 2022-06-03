from setuptools import setup, find_packages

setup(
    name='auth',
    version='1.0',
    long_description=__doc__,
    packages=['auth'],
    include_package_data=True,
    zip_safe=False,
    install_requires=[
        'Flask-Login==0.6.1',
        'Flask==2.1.0',
        'mysql-connector==2.2.9',
        'oauthlib==3.1.0',
        'pymysql==0.9.3',
        'requests==2.22.0',
        'DBUtils==1.3',
        'pyOpenSSL==19.1.0',
    ]
)