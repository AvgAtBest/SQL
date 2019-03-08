<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	
	$password = $_POST["passwordPost"];
	$email = $_POST["emailPost"];
	
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
	
	if(!$conn)
	{
		die("Conneciton Failed.". mysqli_connect_error());
	}
	
	$sqlUpdatePassword = "UPDATE users SET password = '".$password."'WHERE email = '".$email."'";
	$resultChangePassword = mysqli_query($conn, $sqlUpdatePassword);
	if(!resultChangePassword){
		echo "Error";
	}
	else{
		echo "password change";
	}


?>