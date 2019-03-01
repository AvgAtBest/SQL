<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	
	$username = $_POST["usernamePost"];
	$password = $_POST["passwordPost"];
	//$email = $_POST["emailPost"];
	
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
	
	if(!$conn)
	{
		die("Conneciton Failed.". mysqli_connect_error());
	}
	$finduser = "SELECT password FROM users WHERE username = '".$username."' ";
	$finduserresult = mysqli_query($conn, $finduser);

	
	//Get the result and confirm login
	if(mysqli_num_rows($finduserresult)>0)
	{
		//search through the rows
		while($row = mysqli_fetch_assoc($finduserresult))
		{
			if($row['password'] == $password)
			{
				echo "Login success";
				echo $row['password'];
			}
			else
			{
				echo "password incorect";
				echo "password is = ". $row['password'];
			}
		}
	} else {
		echo "user not found";
		//echo "password is = ". $row['password'];
	}


?>