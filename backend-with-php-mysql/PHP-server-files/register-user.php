<?php
require './db.php';

// Check connection
if ($conn->connect_error)
    die('Connection failed: ' . $conn->connect_error);

// variables submited by user
$loginUser = $_POST['loginUser'];
$loginPass = $_POST['loginPass'];
$confirmPass = $_POST['confirmPass'];

// Prepared Statement
$sql = "SELECT username FROM users WHERE username = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $loginUser);

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        echo "Username is already taken.";
    }
}
else if ($loginPass !== $confirmPass || empty($loginPass) || empty($confirmPass)) {
    echo "inputed passwords are not equal or has null";
} else {
    echo 'Creating user...';

    $sql2 = "INSERT INTO users (username, password, level, coins) VALUES (?, ?, 1, 0)";
    $stmt2 = $conn->prepare($sql2);
    $stmt2->bind_param("ss", $loginUser, $loginPass);

    if ($stmt2->execute() === TRUE) 
        echo "New record created successfully";
    else 
        echo "Error: " . $sql2 . "<br>" . $conn->error;
      
}
$stmt2->close();
$stmt->close();
$conn->close();
?>
