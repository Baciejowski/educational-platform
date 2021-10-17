module.exports = {
    devServer: {
        // port: 8080,
        proxy: {
            "^/api/external": {
                target: "http://localhost:3001",
                changeOrigin: true,
                logLevel: "debug",
                pathRewrite: { "^/api": "/api" }
            },
            "^/api": {
                target: "http://localhost:5000",
                changeOrigin: true,
                logLevel: "debug",
                pathRewrite: { "^/api": "/api" }
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
