<?php
class Session{
    public static function exist($name){
        if(isset($_SESSION[$name]))
            return true;
        return false;
    }
    public static function get($name){
        if(isset($_SESSION[$name]))
            return $_SESSION[$name];
        return false;
    }
    public static function set($name,$value){
        $_SESSION[$name] = $value;
    }
    public static function delete($name){
        if(isset($_SESSION[$name]))
            unset($_SESSION[$name]);
        return true;
    }
    public static function CreateSessionId(){
        $firstKey = "";
        $keys = "abcdefghijklmnopqwestrxyz1234567890";
        $length = strlen($keys) - 1;
        for($i=0;$i<32;$i++)
            $firstKey .= $keys[rand(0,$length)];
        return hash("MD5",$firstKey);
    }
    public static function SetSessionId($SessionIdStr,$username)
    {
        $DBUsernameHandle = DataBase::getInstance()->get("username","Users", array("username","=",$username));
        if($DBUsernameHandle->getCount() != 1)
            return false;
        $DBUpdateHandle = DataBase::getInstance()->update("Users",array("SessionId"),array($SessionIdStr),array("username","=",$username));
        if($DBUpdateHandle->hasError()){
            echo $DBUpdateHandle->getError()[0]["message"];
            return false;
        }
        return true;
    }

};