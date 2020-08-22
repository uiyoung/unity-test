<?php
require './db.php';

// Check connection
if ($conn->connect_error)
    die('Connection failed: ' . $conn->connect_error);

// variables submited by user
$itemID = $_POST["itemID"];

// Prepared Statement
$sql = "SELECT name, description, price, imgVer FROM items WHERE id = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $itemID);

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    $rows = array();
    while ($row = $result->fetch_assoc()) {
        $rows[] = $row;
    }
    echo json_encode($rows);
} else 
    echo '0';

$stmt->close();
$conn->close();
?>