<?php
session_start();
if(isset($_SESSION["user_id"]))
  header("Location: http://192.168.1.87/lobby");
include_once "Home/index.php";
die();
