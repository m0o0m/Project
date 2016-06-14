<?php
/*
 * database connecting to sql server
 * there is some method to transport data between database and web app
 */

class DataBase{
    private static $instance = NULL;
    private $conn = NULL;

    private $error;
    private $err;
    private $results;
    private $count;

    public static function getInstance(){
        if(self::$instance == NULL)
            self::$instance = new DataBase();
        if(self::$instance != NULL){
            if(self::$instance->conn == NULL)
            {
                $connectionInfo = array("Database" => DATABASE_NAME );
                self::$instance->conn = sqlsrv_connect(SERVER_NAME,$connectionInfo);
            }
        }
        if(self::$instance && self::$instance->conn)
            return self::$instance;
        echo "Connection could not be established.<br/>";
        die( print_r( sqlsrv_errors(), true));
    }
    public function get($col,$table,$where)
    {
        $this->err = "";
        $this->error = false;
        $this->results = "";
        $this->count = 0;
        if (!is_array($where)) {
            $this->error = true;
            $this->err = "bad parameter used on get function!";
            return this;
        }
        if (count($where) != 3) {
            $this->error = true;
            $this->err = "bad parameter used on get function!";
            return this;
        }
        $operators = array("=", "!=", ">", "<", ">=", "<=");
        if (!in_array($where[1], $operators)) {
            $this->error = true;
            $this->err = "bad operator used on get function!";
            return this;
        }
        $query = "SELECT " . $col . " FROM " . $table . " WHERE " . $where[0] . $where[1] . "'" . $where[2] . "'";

        $params = array();
        $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );

        $stmt = sqlsrv_query($this->conn, $query , $params, $options);

        if($stmt) {
            $this->results = sqlsrv_fetch_array($stmt);
            $this->count = sqlsrv_num_rows($stmt);
        }
        return $this;
    }
    public function set($table,$columns,$values){
        if(!is_array($columns) || !is_array($values))
            return $this;
        $length = 0;
        if(($length = count($columns)) != count($values) || $length == 0) {
            return $this;
        }
        $query = "INSERT INTO ".$table." (";
        $query.= $columns[0];
        for($i = 1; $i < $length ; $i++)
            $query .= ",".$columns[$i];
        $query .= ") VALUES ('".$values[0]."'";
        for($i = 1; $i < $length ; $i++)
            $query .= ",'".$values[$i]."'";
        $query .=  ")";

        if(sqlsrv_query($this->conn,$query))
            return true;
        else
        {
            var_dump(sqlsrv_errors());
            return false;
        }
    }
    public function delete(){}
    public function update($table,$columns,$values,$where){
        if(!is_array($where))
        {
            $this->error = true;
            $this->err = "bad parameter used on get function!";
            return this;
        }
        $length = 0;
        if(count($where) != 3 || ($length = count($columns)) != count($values) || $length <= 0 )
        {
            $this->error = true;
            $this->err = "bad parameter used on get function!";
            return this;
        }
        $operators = array("=" , "!=" , ">" , "<" , ">=" , "<=");
        if(!in_array($where[1],$operators)){
            $this->error = true;
            $this->err   = "bad operator used on get function!";
            return this;
        }

        $query = "UPDATE ".$table." SET ";
        $query.= $columns[0] ."='".$values[0]."'";

        for($i=1;$i<$length;$i++)
            $query .= ",". $columns[$i] ."='".$values[0]."'";
        $query .= " WHERE ".$where[0].$where[1]."'".$where[2]."'";

        if(!sqlsrv_query($this->conn,$query))
        {
            $this->err = sqlsrv_errors();
            $this->error = 1;
            return $this;
        }
        return $this;
    }
    public function createTable($tableName){}
    public function hasError(){
        return $this->error;
    }
    public function getError(){
        return $this->err;
    }
    public function getResult(){
        return $this->results;
    }
    public function getCount(){
        return $this->count;
    }
}
