<?php
session_start();
include_once "../config.php";
if(Session::exist("user_id"))
{
    $GLOBALS["session_id"] = Session::CreateSessionId();
    Session::SetSessionId($GLOBALS["session_id"],Session::get("user_id"));
    include_once "includes/lobby.php";
}
else
    header("Location: ".$GLOBALS["home_url"]."/login.php");