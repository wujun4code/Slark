'use strict';
var rxjs = require('rxjs');
var utils = require('./utils');
var rxLeanCloud = require('rx-lean-js-core');
var RxAVQuery = rxLeanCloud.RxAVQuery;
var RxAVObject = rxLeanCloud.RxAVObject;
var RxAVUser = rxLeanCloud.RxAVUser;

var groupProperties = {
    className: 'ChatGroup',
    name: 'name',
    id: 'objectId',
};

var userGroupRelationProperties = {
    className: 'User_ChatGroup',
    user: 'user',
    group: 'group',
    markName: 'markName'
};

class xGroupService {

    query(groupId) {
        let query = new RxAVQuery(userGroupRelationProperties.className);
        let groupObj = RxAVObject.createWithoutData(groupProperties.className, groupId);
        query.equalTo(userGroupRelationProperties.group, groupObj);
        return query.find().map(list => {
            return list.map(middle => {
                let user = middle.get(userGroupRelationProperties.user);
                if (user != undefined) {
                    return user.objectId;
                }
            });
        });
    }
}

module.exports = xGroupService;

module.exports = {
    xGroupService: xGroupService,
    groupProperties: groupProperties,
    userGroupRelationProperties: userGroupRelationProperties
};