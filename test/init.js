var RxAVClient = require('rx-lean-js-core').RxAVClient;
var RxAVApp = require('rx-lean-js-core').RxAVApp;
var appId = 'fGn0RfnxAHyl6yAsxaFHVucg-gzGzoHsz';
var appKey = 'W7tppJardfWCqLx8VVs8c6Vv';

var app = new RxAVApp({
    appId: appId,
    appKey: appKey,
});

function initApp() {
    RxAVClient.init().add(app);
}

module.exports = {
    initApp: initApp
};