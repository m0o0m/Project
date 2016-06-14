/**
 * Created by mmdamin on 4/15/2016.
 */
var IpAddress = "127.0.0.1";
var url = "http://127.0.0.1/golden/";
var conn = null;
var connected = false;
var OpenTables;
var UserName;

$(document).ready(function(){
    connect();
    fixStyle();
    $(window).resize(function(){fixStyle();});
    loading();
    LoadProfileImage();
});