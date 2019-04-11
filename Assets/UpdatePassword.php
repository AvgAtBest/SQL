<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	
	$password = $_POST["passwordPost"];
	$email = $_POST["emailPost"];
	//establishes connection to database
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
	//if it fails to gain a connection to the database
	if(!$conn)
	{
		//kill connection, return connection failed
		die("Conneciton Failed.". mysqli_connect_error());
	}
	//Updates password in users when it finds a matching email from the matching data sent from Login Script
	$sqlUpdatePassword = "UPDATE users SET password = '".$password."'WHERE email = '".$email."'";
	$resultChangePassword = mysqli_query($conn, $sqlUpdatePassword);//sends result
	//if it cant change the password
	if(!resultChangePassword){
		echo "Error";//echo error, dont change database
	}
	else{
		echo "password change";
		//password change success
	}


?>