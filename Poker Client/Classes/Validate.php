<?php
//validating functions working with sqlsrv database

define("MIN_USERNAME_LENGTH",4);
define("MAX_USERNAME_LENGTH",20);
define("MIN_PASSWORD_LENGTH",4);
define("MAX_PASSWORD_LENGTH",20);

class Validate
{
    //return true if password and username are correct
    //return false if it is not
    public static function login($password,$username)
    {
        $DBHandleUsername = DataBase::getInstance()->get("username","Users",array("username","=",$username));
        if($DBHandleUsername->getCount() != 1)
            return false;
        $DBHandlePassword = DataBase::getInstance()->get("password","Users",array("username","=",$username));
        if($DBHandlePassword->getCount() != 1)
            return false;
        $res = $DBHandlePassword->getResult();
        if(!isset($res["password"]))
            return false;
        $DBHandleSalt = DataBase::getInstance()->get("salt","Users",array("username","=",$username));
        if($DBHandleSalt->getCount() != 1)
            return false;
        $res2 = $DBHandleSalt->getResult();
        if(!isset($res2["salt"]))
            return false;
        $password = hash("MD5",$password + $res2["salt"]);

        if($res["password"] != $password)
            return false;
        return true;
    }
    public static function checkNewUsername($username){
        //checking database
        $count = DataBase::getInstance()->get("username","users",array("username","=",$username))->getCount();
        if($count >= 1){
            return false;
        }
        //checking username
        $len = strlen($username);

        if ($len < MIN_USERNAME_LENGTH )
            return false;
        if($len > MAX_USERNAME_LENGTH )
            return false;
        $username = str_split($username);
        foreach($username as $ch)
            if( !( ( $ch <= 'z' && $ch >= 'a')||($ch <= 'Z' && $ch >= 'A')||($ch >= '0' && $ch <= '9') ) ){
                return false;
            }
        return true;
    }
    public static function checkEmail($email){
        $count = DataBase::getInstance()->get("username","users",array("email","=",$email))->getCount();
        if($count>=1)
            return false;
        return true;
    }
    public static function checkPass($password,$passwordAgain){
        if(strlen($password) != strlen($passwordAgain))
            return false;
        if( !(strlen($password) > MIN_PASSWORD_LENGTH) )
            return false;
        if(!(strlen($password) < MAX_PASSWORD_LENGTH))
            return false;
        $password = str_split($password);
        $passwordAgain = str_split($passwordAgain);
        $i = 0;
        foreach($password as $ch){
           if($ch != $passwordAgain[$i++])
                return false;
        }
        return true;
    }
    public static function checkValidateID($validateID){

    }
    public static function createValidatorID($username)
    {
        $ValidateID = "123456";
        Database::getInstance()->update("ValidateID",$ValidateID,"users",array("username","=",$username));
        $emailAddress = Database::getInstance()->get("email","users",array("username","=",$username));
        $subject = "Don't Reply";
        $message = "Hi it is your validate Code : ".$ValidateID;
        //mail($emailAddress,$subject,$message);
    }
}
