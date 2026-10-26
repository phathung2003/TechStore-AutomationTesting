@extends('client.layouts.master')
@section('content')
<div class="container" style="max-width: 1270px">
    <div class="row">
        <div class="center-content">
            <div class="blog-content">
                <div class="info" style="float: left">
                    <span style="color:#B82121; font-weight:bold">Get in touch <span style="color: black">with us</span></span>
                    <p style=" text-align:left; margin-top:20px; line-height:1.5; color:black">Get in touch to discuss your employee wellbeing needs today. Please give us a call, drop us an email or fill out the contact form and we’ll get back to you.</p>
                    <div class="info-contact" style="margin-top: 10%">
                        <div>
                            <span class="material-symbols-outlined">
                                home_pin
                            </span>
                        </div>
                        <div class="location">
                            <h4 style="font-weight: bold; color: black">Headquater</h4>
                            <p style="line-height: 1.5">10 National Highway 22, Tan Xuan commune, Hoc Mon district, Ho Chi Minh City</p>
                        </div>
                    </div>

                    <div class="info-contact" style="margin-top: 5%">
                        <div>
                            <span class="material-symbols-outlined">
                                support_agent
                            </span>
                        </div>
                        <div class="location">
                            <h4 style="font-weight: bold; color: black">Phone Number</h4>
                            <p style="margin-bottom: 10px">Mobile: 0913 456 789</p>
                            <p>Hot line: 1900 0000</p>
                        </div>
                    </div>

                    <div class="info-contact" style="margin-top: 5%">
                        <div>
                            <span class="material-symbols-outlined">
                                contact_mail
                            </span>
                        </div>
                        <div class="location">
                            <h4 style="font-weight: bold; color: black">Email Us</h4>
                            <p style="margin-bottom: 10px">Hello@gmail.com</p>
                        </div>
                    </div>
                </div>

            </div>
            <div class="img">
                <img style="width:100%" src="{{asset('images/ct2-pic.png')}}" alt="">
            </div>

            <div class="about-us">
                <div class="info1" style=" border-right: 1px solid white">
                    <h1 style="color: white; font-weight:bold">TechStore</h1>
                    <p style="color:white; line-height:1.3">We provide leading mobile technology products, from mobile phones to accessories and repair services.</p>
                    <a style="color: white; " href="#" class="fa fa-facebook"></a>
                    <a style="color: white;" href="#" class="fa fa-twitter"></a>
                    <a style="color: white; " href="#" class="fa fa-instagram"></a>
                    <a style="color: white; " href="#" class="fa fa-pinterest"></a>
                </div>

                <div class="info" style=" color:white; border-right: 1px solid white">
                    <h4 style="font-weight:bold; color:white">Get in touch</h4>
                    <p style="line-height: 1.3; margin: 0 0 10px">10 National Highway 22, Tan Xuan commune, Hoc Mon district, Ho Chi Minh City</p>
                    <p style="margin-bottom: 10px">Hello@gmail.com</p>
                    <p style="margin-bottom: 10px">0913 456 789</p>
                </div>

                <div class="info" style=" color:white; border-right: 1px solid white">
                    <h4 style="font-weight:bold; color:white">Offer Information</h4>
                    <a href="#" style="color: white; font-weight:bold; margin-bottom:10px; display:block">Help center</a>
                    <a href="#" style="color: white; font-weight:bold; margin-bottom:10px; display:block">Warranty service</a>
                    <a href="#" style="color: white; font-weight:bold; margin-bottom:10px; display:block">Operation regulations</a>
                </div>

                <div class="info" style=" color:white">
                    <h4 style="font-weight:bold; color:white">Information and policies</h4>
                    <a href="#" style="color: white; font-weight:bold; margin-bottom:10px; display:block">Buy and pay online</a>
                    <a href="#" style="color: white; font-weight:bold; margin-bottom:10px; display:block">Buy installment goods online</a>
                    <a href="#" style="color: white; font-weight:bold; margin-bottom:10px; display:block">Delivery Policy</a>
                    <a href="#" style="color: white; font-weight:bold; margin-bottom:10px; display:block">Look up warranty information</a>
                </div>
            </div>
        </div>
    </div>
</div>
@endsection

<style>
    .blog-content .info span {
        font-size: 40px;
    }

    .row {
        display: flex;
        justify-content: center;
    }

    .about-us .info {
        margin-top: 30px;
        width: 20%;
    }

    .about-us .info1 {
        margin-top: 30px;
        width: 20%;
    }

    .about-us .info1 p{
        margin-top: 20px;
    }

    .about-us .info1 a{
       font-size: 24px;
       margin: 10px 10px 20px 10px;
    }

    .center-content {
        width: 90%;
        height: 100%;
        align-content: center;
        background: white;
        border-radius: 0 0 15px 15px;
        border: 1px solid lightgrey;
        box-shadow: 10px 0 10px -5px lightgray, 0 10px 10px -5px lightgray, -10px 0 10px -5px lightgray, 0 -10px 10px -5px white;
    }

    .intro {
        background-image: url('images/blog.jpg');
        background-size:cover;
        height: 200px;
        position: relative;
        display: block;
        background-position: center;
    }

    .info {
        top:8%;
        margin-left: 40px;
        width: 70%;
    }

    .contact {
        top: 50%;
        position: inherit;;
        left:50%;
        Transform: translate(-50%, -50%);
        font-weight: 600;
        color: black;
    }

    .path {
        margin-top: 20px;
    }

    .info span.material-symbols-outlined {
        color: transparent;
        display: inline-block;
        background-image: linear-gradient(to bottom, #B82121 50%, orange);
        background-clip: text;
        -webkit-background-clip: text;
        color: transparent;
        font-size: 40px;
    }
    .path a{
        font-size:large;
        color: gray;
        margin: 0 10px 0 10px
    }

    .blog-content {
        margin-top: 8%;
        display: inline-block;
        width: 50%;
    }

    .info-contact {
        display: flex;
    }

    .info-contact .location {
        margin-left: 24px;
    }

    .center-content .img {

        float: right;
        width: 50%;
        display: inline-block;
    }

    .about-us {
        width: 100%;
        display: flex;
        background-color: #B82121;
        border-radius: 0 0 15px 15px;
        justify-content: center;
    }

    .about-us .info {
        margin-top: 30px;
    }

    @media screen and (max-width:1160px){
        .about-us h4 {
            font-size:larger;
        }

        .about-us h1 {
            font-size:30px;
        }

        .about-us div {
            padding: 20px;
            margin: 10px;
        }

        .about-us .info1 {
            padding-left: 0px;
        }

        .about-us .info {
            padding-left: 0px;
        }

        .about-us .info1 a {
            font-size: 20px;
            margin: 10px 5px 20px 5px;
        }
    }

    @media (max-width:930px) {
        .about-us h1 {
            font-size: 20px;
        }

        .about-us .info1 a {
            font-size: 15px;
        }
    }

    @media (max-width:751px) {
        .blog-content {
            width: 100%;
        }

        .blog-content .info {
            width: 80%;
        }

        .blog-content .info span {
            font-size: 35px;
        }

        .blog-content .info p{
            font-size: 20px;
        }

        .center-content .img {
            float: left;
            margin: 15px 0 15px 35px;
            width: 80%;
        }
        .about-us h4 {
            font-size:medium;
        }

        .about-us h1 {
            font-size:20px;
        }

        .about-us .info {
            padding: 2px;
            margin: 25px 5px 0 5px;
            width: 22%;
        }

        .about-us .info1 {
            padding: 0;
            margin-right: 0;
            width: 22%;
        }

        .about-us p {
            font-size: 13px;
        }

        .about-us a {
            font-size: 13px;
        }

        .about-us .info1 a{
            font-size: 13px;
            margin: 5px 5px 10px 0;
        }

        .info span.material-symbols-outlined {
            font-size: 30px;
        }

        .location h4 {
            font-size: 25px;
        }

        .location p {
            font-size: 20px;
        }
    }

    @media screen and (max-width:500px){
        .blog-content .info span {
            font-size: 30px;
        }

        .blog-content .info p{
            font-size: 15px;
        }

        .location h4 {
            font-size: 20px;
        }

        .location p {
            font-size: 10px;
        }

        .about-us .info1 h1{
            font-size: 15px;
        }

        .about-us .info1 p{
            margin-top: 10px;
            font-size: 12px;
        }

        .about-us .info h4{
            font-size: 14px;
        }

        .about-us .info p{
            font-size: 10px;
        }

        .about-us .info a {
            font-size: 11px;
            font-weight: normal;
        }
    }
</style>
