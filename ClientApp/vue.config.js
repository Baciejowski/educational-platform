module.exports = {
    devServer: {
        // port: 8080,
        proxy: {
            "^/api/classes": {
                target: "http://localhost:5000",
                changeOrigin: true,
                logLevel: "debug",
                pathRewrite: { "^/api/classes": "http://localhost:5000/api/classes" }
            },

            "^/weatherforecast": {
                target: "http://localhost:5000",
                changeOrigin: true,
                logLevel: "debug",
                pathRewrite: { "^/weatherforecast": "http://localhost:5000/weatherforecast" }
            }
        }
    }
};
