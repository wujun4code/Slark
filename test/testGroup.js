'use strict';

var rxjs = require('rxjs');
var rxLeanCloud = require('rx-lean-js-core');
var RxAVQuery = rxLeanCloud.RxAVQuery;
var RxAVObject = rxLeanCloud.RxAVObject;
var RxAVUser = rxLeanCloud.RxAVUser;
var groupSrc = require('../group');
var initApp = require('./init').initApp;

initApp();

describe('group', () => {
    it('create', done => {
        let u = undefined;
        RxAVUser.logIn('junwu', 'leancloud').flatMap(user => {
            u = user;
            let newGroup = new RxAVObject(groupSrc.groupProperties.className);
            newGroup.set('name', 'mochaTest');
            return newGroup.save();
        }).flatMap(groupObj => {
            let user_group = new RxAVObject(groupSrc.userGroupRelationProperties.className);
            user_group.set(groupSrc.userGroupRelationProperties.user, u);
            user_group.set(groupSrc.userGroupRelationProperties.group, groupObj);
            return user_group.save();
        }).subscribe(joined => {
            console.log('joined', joined);
            done();
        });

    });
});

