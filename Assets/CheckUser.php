<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	

	$email = $_POST["emailPost"];
	
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
	
	if(!$conn)
	{
		die("Conneciton Failed.". mysqli_connect_error());
	}
	while($row = mysqli_fetch_assoc($result))
	{
		echo $row['username'];
	}
	else
	{
		echo "No user";
	}

?>