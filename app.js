'use strict';
var express = require('express');
var path = require('path');
var AV = require('leanengine');

var app = express();
app.use(AV.express());

module.exports = app;