<?php
$servername = "localhost";
$username = "root";
$password = "123456";
$dbName = "sqltest";
//Make Connection
$conn = new mysqli($servername, $username, $password, $dbName);

if(!$conn){
	die("Connection Failed.". mysqli_connect_error());
}
else echo("Connection Success");
?>