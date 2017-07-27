'use strict';
var rxjs = require('rxjs');
var utils = require('./utils');

class xMessage {
    constructor(data) {
        this.data = data;
        this.type = data.type;
        this.id = utils.newObjectId();
    }

    textMessage() {

    }

    pushData() {

        let title = '您有一条未读消息';
        let body = '';
        switch (this.type) {
            case 'text': {
                body = this.data.content;
                break;
            }

            case 'picture':
            case 'image': {
                break;
            }

            default: {
                break;
            }
        }
        
        let alert = {
            title: title,
            body: body
        };

        return {
            alert: alert,
            sound: 'default',
            id: this.id
        };
    }

    directData() {
        return {
            metaData: this.data,
            id: this.id
        };
    }

    send(from, to) {
    }

}

module.exports = xMessage;