<?php

define("SERVER_NAME","DESKTOP-1L1I0GO");
define("DATABASE_NAME","GoldenDB");

$GLOBALS["session_id"] = "";
$GLOBALS["serverIp"] = "192.168.1.87";
$GLOBALS["home_url"]   = "http://".$GLOBALS["serverIp"];
$GLOBALS["game_url"]   = $GLOBALS["home_url"]."/Lobby";
$GLOBALS["lobby_title"] = "Lobby";

$GLOBALS["home_path"] = __DIR__;
$GLOBALS["defaults_path"] = $GLOBALS["home_path"]. "\\Defaults";
$GLOBALS["profiles_path"] = $GLOBALS["home_path"] . "\\UsersData";

function autoload($class)
{
    include_once "Classes\\".$class.".php";
}
spl_autoload_register("autoload");
