const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
    ? env.ASPNETCORE_URLS.split(";")[0]
    : "https://localhost:5001";

const PROXY_CONFIG = [
    {
        context: ["/api", "/Identity", "/swagger"],
        proxyTimeout: 30000,
        target: target,
        secure: false,
        headers: {
            Connection: "Keep-Alive",
        },
        logLevel: "debug",
    },
];

module.exports = PROXY_CONFIG;
