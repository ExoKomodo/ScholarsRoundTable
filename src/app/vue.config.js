const path = require('path');
const fs = require('fs')

module.exports = {
    configureWebpack: {
        devtool: 'source-map'
    },
    outputDir: path.resolve(__dirname, '../../dist'),
    assetsDir: './static',
    devServer: {
        https: false,
        public: 'https://localhost:8080/',
        key: fs.readFileSync('../ssl/localhost/localhost.key'),
        cert: fs.readFileSync('../ssl/localhost/localhost.crt'),
        ca: fs.readFileSync('../ssl/localhost/myCA.pem'),
    },
}
