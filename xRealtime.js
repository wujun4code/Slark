'use strict';
var xConnectionService = require('./connection');
var xMessageService = require('./message');
var xGroupService = require('./group').xGroupService;

class xRealtime {
    constructor(io) {
        this.group = new xGroupService();
        this.connection = new xConnectionService(io);
        this.message = new xMessageService(this.connection, this.group);
    }

    start() {
        this.connection.start(this.message);
    }
}

module.exports = xRealtime;