﻿<br/>
<p align="center">
  <h3 align="center">Image Generator API</h3>

  <p align="center">
    Image Generator API using Dall-E, C# ASP .NET, oAuth & Json Web Token
    <br/>
    <br/>
    <a href="https://github.com/manesjonathan/image-generator-.NET/issues">Report Bug</a>
    .
    <a href="https://github.com/manesjonathan/image-generator-.NET/issues">Request Feature</a>
  </p>

![Downloads](https://img.shields.io/github/downloads/manesjonathan/image-generator-.NET/total) ![Contributors](https://img.shields.io/github/contributors/manesjonathan/image-generator-.NET?color=dark-green) ![Forks](https://img.shields.io/github/forks/manesjonathan/image-generator-.NET?style=social) ![Stargazers](https://img.shields.io/github/stars/manesjonathan/image-generator-.NET?style=social) ![Issues](https://img.shields.io/github/issues/manesjonathan/image-generator-.NET)

## About The Project

![Screen Shot](demo.gif)

This project is created using C# ASP .NET and OpenAI Dall-E API to generate image.

This project use oAuth to log in users & using Json Web Token to validate requests. The users are registered in a
Postgresql
database.

It also uses AWS S3 to store the generated images.

At creation, the registered user has 1 credit. Each time he generates an image, he loses 1 credit.
but there is an option to refill the credits by paying with Stripe (1€ / 5 credits).

## Getting Started

To get a local copy up and running follow these simple example steps.

### Installation

1. Get an API Key at [https://openai.com/]

2. Configure AWS IAM user, S3 and create a bucket. You will need the bucket name, region, access key and secret key.

3. Create a Stripe account and get the secret key.

4. Create a Postgresql database.

5. Clone the repo

```sh
git clone https://github.com/manesjonathan/image-generator-.NET.git
```

6. Enter your credentials in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DatabaseURL": "Host=[DB_URL]; Database=[DB_NAME]; Username=[DB_USER]; Password=[DB_PASS]",
    "StripeApiKey": "[SK_TEST]",
    "StripePublishableKey": "[PK_TEST]",
    "JWTSecret": "[JWT_SECRET]",
    "WebhookSecret": "[WH_SECRET]",
    "OpenAiApiKey": "[OPENAI_API_KEY]",
    "AWSAccessKey": "[AWS_ACCESS_KEY]",
    "AWSSecretKey": "[AWS_SECRET_KEY]"
  }
}
```

6. Create the Stripe webhook in your Stripe account. If you run locally, you can use [ngrok](https://ngrok.com/) to
   create a public URL for your localhost. You need to configure webhook for the following event: `payment_intent.succeeded`

7. Run the project

## Contributing

The following features are planned for the next release:

* Add a feature to rate the generated images

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any
contributions you make are **greatly appreciated**.

* If you have suggestions for adding or removing projects, feel free
  to [open an issue](https://github.com/manesjonathan/image-generator-.NET/issues/new) to discuss it.
* Please make sure you check your spelling and grammar.
* Create individual PR for each suggestion.

### Creating A Pull Request

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Authors

* [Jonathan Manes](https://github.com/manesjonathan/) - *Full Stack Developer*
