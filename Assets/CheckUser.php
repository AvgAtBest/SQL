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
	$checkuser = "SELECT username FROM users WHERE email = '".$email."'";
	$checkuserresult = mysqli_query($conn, $checkuser);
	if(mysqli_num_rows($checkuserresult) > 0)
	{
		while($row = mysqli_fetch_assoc($checkuserresult))
		{
			//echo $row['username'];
			echo "user found";
		}
	}
	else
	{
		echo "no user";
	}


?>