# Blog API

# Getting Started

<strong>Supports MySQL</strong>
<br><br>
<strong>Requires .NET 6</strong>
<br><br>
Run the SQL script or create database based on the connection string and models.
<br><br>
Change the ```User``` and ```Password``` fields in ```appsettings.json``` to match your local database user information.

# ```GET``` Endpoints


#### ```GET``` All Users
```
https://localhost:7201/api/user
```
Sample Response: 
```
{
	"statusCode": 200,
	"statusDescription": "Users found",
	"users": [
		{
			"userId": 1,
			"username": "Apple123",
			"email": "Apple123@email.com",
			"dateJoined": "2022-11-25T16:47:37",
			"aboutUser": "I like apples",
			"posts": [
				{
					"postId": 1,
					"title": "Why I like Apples",
					"content": "I like apples not only because they are crunchy and delicious but also because they are highly nutritious.",
					"dateCreated": "2022-11-25T16:58:08",
					"dateUpdated": "2022-11-27T23:34:04",
					"userId": 1
				},
			]
		},
		{
			"userId": 2,
			"username": "vroom",
			"dateJoined": "2022-11-27T23:49:41",
			"posts": [
				{
					"postId": 7,
					"title": "rocks on the beach",
					"content": "common beach rocks are agates, basalt, conglomerate, granite, slate, rhyolite, and quartzite.",
					"dateCreated": "2022-11-27T23:51:25",
					"userId": 2
				}
			]
		}
	]
}
```
#### ```GET``` All Posts
```
https://localhost:7201/api/post
```
Sample Response:
```
{
	"statusCode": 200,
	"statusDescription": "Posts found",
	"posts": [
		{
			"postId": 1,
			"title": "Why I like Apples",
			"content": "I like apples not only because they are crunchy and delicious but also because they are highly nutritious.",
			"dateCreated": "2022-11-25T16:58:08",
			"dateUpdated": "2022-11-27T23:34:04",
			"userId": 1
		},
		{
			"postId": 4,
			"title": "Tropical Sunshine",
			"content": "tut-tut-trot-low-high",
			"dateCreated": "2022-11-27T01:17:55",
			"dateUpdated": "2022-11-27T23:47:23",
			"userId": 1
		},
	]
}
```
#### ```GET``` User by ID
```
https://localhost:7201/api/user/{userId}
```
Sample Response:
```
{
	"statusCode": 200,
	"statusDescription": "User id 1 found",
	"user": {
		"userId": 1,
		"username": "Apple123",
		"email": "Apple123@email.com",
		"dateJoined": "2022-11-25T16:47:37",
		"aboutUser": "I like apples",
		"posts": [
			{
				"postId": 1,
				"title": "Why I like Apples",
				"content": "I like apples not only because they are crunchy and delicious but also because they are highly nutritious.",
				"dateCreated": "2022-11-25T16:58:08",
				"dateUpdated": "2022-11-27T23:34:04",
				"userId": 1
			},
		]
	}
}
```
#### ```GET``` Post by ID
```
https://localhost:7201/api/post/{postId}
```
Sample Response:
```
{
	"statusCode": 200,
	"statusDescription": "Post of id 1 found",
	"post": {
		"postId": 1,
		"title": "Why I like Apples",
		"content": "I like apples not only because they are crunchy and delicious but also because they are highly nutritious.",
		"dateCreated": "2022-11-25T16:58:08",
		"dateUpdated": "2022-11-27T23:34:04",
		"userId": 1
	}
}
```
#### ```GET``` User Post by Post Number
```
https://localhost:7201/api/user/{userId}/post/{postNum}
```
Sample Response:
```
{
	"statusCode": 200,
	"statusDescription": "User found",
	"user": {
		"userId": 1,
		"username": "Apple123",
		"email": "Apple123@email.com",
		"dateJoined": "2022-11-25T16:47:37",
		"aboutUser": "I like apples",
		"posts": [
			{
				"postId": 1,
				"title": "Why I like Apples",
				"content": "I like apples not only because they are crunchy and delicious but also because they are highly nutritious.",
				"dateCreated": "2022-11-25T16:58:08",
				"dateUpdated": "2022-11-27T23:34:04",
				"userId": 1
			}
		]
	}
}
```


# ```POST``` Endpoints


#### ```POST``` Create a User
```
https://localhost:7201/api/user/
```
Sample Body
```
{
	"username": "Turnip",
	"aboutUser": "I love turnips"
}
```
Sample Response
```
{
	"statusCode": 201,
	"statusDescription": "User Turnip has been successfully created"
}
```
#### ```POST``` Create a Post For User
```
https://localhost:7201/api/user/{userId}/post
```
Sample Body
```
{
	"title": "Why Turnips are Great",
	"content": "Turnips are a cruciferous vegetable with multiple health benefits. They boast an impressive nutritional profile, and their bioactive compounds..."
}
```
Sample Response
```
{
	"statusCode": 201,
	"statusDescription": "Post for user of id 10 created"
}
```


# ```PUT``` Endpoints


#### ```PUT``` Update a User's Information
```
https://localhost:7201/api/user/{userId}
```
Sample Body
```
{
	"aboutUser": "I don't like turnips anymore..."
}
```
Sample Response
```
{
	"statusCode": 204,
	"statusDescription": "Update has succeeded"
}
```
#### ```PUT``` Update a User's Posts' Information
```
https://localhost:7201/api/user/{userId}/post/{postNum}
```
Sample Body
```
{
	"title": "Why Turnips are awful",
	"content": "Turnips are awful, they're cold and hard and often small and bitter..."
}
```
Sample Response
```
{
	"statusCode": 204,
	"statusDescription": "Updated post 1 for User 10"
}
```


# ```DELETE``` Endpoints


#### ```DELETE``` User by ID
```
https://localhost:7201/api/post/{id}
```
Sample Response 
```
{
    "statusCode": 204,
    "statusDescription: "User of id 10 successfully deleted"
}
```
#### ```DELETE``` Post by ID
```
https://localhost:7201/api/post/{id}
```
Sample Response
```
{
    "statusCode": 204,
    "statusDescription: "Post of id 10 was removed"
}
```

#### ```DELETE``` a User's Post by Post Number
```
https://localhost:7201/api/user/{userId}/post/{postNum}
```
Sample Response 
```
{
    "statusCode": 204,
    "statusDescription: "Post 1 successfully deleted from user of id 10"
}
```
___
