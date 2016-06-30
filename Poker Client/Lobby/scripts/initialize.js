/**
 * Created by mmdamin on 4/15/2016.
 */
var IpAddress = "192.168.1.87";
var url = "http://192.168.1.87/";
var conn = null;
var connected = false;
var UserName;
var OpenTables;

$(document).ready(function(){
    connect();
    fixStyle();
    $(window).resize(function(){fixStyle();});
    loading();
    LoadProfileImage();
    OpenTables = new TableStorage();
});
