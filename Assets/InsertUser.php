<?php
	$server_name = "localhost";
	$server_user = "root";
	$server_password = "";
	$database_name = "SQueaLsystem";
	
	$username = $_POST["usernamePost"];
	$password = $_POST["passwordPost"];
	$email = $_POST["emailPost"];
	
	$conn = new mysqli($server_name, $server_user, $server_password, $database_name);
	
	if(!$conn)
	{
		die("Conneciton Failed.". mysqli_connect_error());
	}
	$finduser = "SELECT username FROM users";
	$finduserresult = mysqli_query($conn, $finduser);
	$canmakeaccount = "";
	
	//if there are more then 0 rows
	if(mysqli_num_rows($finduserresult)>0)
	{
		//search through the rows
		while($row = mysqli_fetch_assoc($finduserresult))
		{
			if($row['username'] == $username)
			{
				echo "User Already Exists";
			}
			else
			{
				$canmakeaccount = "check email";
			}
		}
	}
	else if(mysqli_num_rows($finduserresult)<=0)
	{
		$makeuser = "INSERT INTO users(username, email, password)
		VALUES('".$username."', '".$email."', '".$password."')";
		$makeuserresult = mysqli_query($conn, $makeuser);
		
		if($makeuserresult)
		{
			echo "Create First User";
		}
	}
	if($canmakeaccount == "check email" && mysqli_num_rows($finduserresult)>0)
	{
		$checkemail = "SELECT email FROM users";
		$checkemailresult = mysqli_query($conn, $checkemail);
		if(mysqli_num_rows($checkemailresult)>0)
		{
			while($row = mysqli_fetch_assoc($checkemailresult))
			{
				if($row['email'] == $email)
				{
					echo "Email Already exists";
				}
				else
				{
					$makeuser = "INSERT INTO users(username, email, password)
					VALUES('".$username."', '".$email."', '".$password."')";
					$makeuserresult = mysqli_query($conn, $makeuser);
		
					if($makeuserresult)
					{
						echo "Create User";
					}
				}
			}
			
		}
	}
?>