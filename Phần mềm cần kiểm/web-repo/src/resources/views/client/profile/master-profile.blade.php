@extends('client.layouts.master')
@section('content')
    <div class="container" style="max-width: 1270px">

        <div class="row">
            <div class="menu-profile" style="position: fixed; top:20%; height:435px; width: 260px; background-color:white; border-radius: 0 25px 25px 0;">
                <div style="border-bottom: 1px solid lightgrey; align-items:center">
                    <div style="display: flex; justify-content:center">
                        <i class="fas fa-user-circle" style="color: black; margin: 25px 15px; font-size:60px; display:block"></i>
                    </div>
                    <div style="display: flex; justify-content:center">
                        <h4 style="font-weight: bold; color: black;">{{Auth::user()->name}}</h4>
                    </div>
                </div>

                <div style="width:260px">
                    <div class="profile_option">
                        <span id="icon" class="material-symbols-outlined">
                            person
                        </span>
                        <a href="{{route('getUserProfile')}}">Account Detail</a>
                    </div>

                    <div class="profile_option">
                        <span id="icon" class="material-symbols-outlined">
                            shopping_cart
                        </span>
                        <a href="{{route('getAllUserOrder',['id'=>Auth()->user()->id ])}}">Order</a>
                    </div>

                    <div class="profile_option">
                        <span id="icon" class="material-symbols-outlined">
                            manage_accounts
                        </span>
                        <a href="{{route('changePassword')}}">Change Password</a>
                    </div>

                    <form id="option-logout" action="{{route('logout')}}" method="POST" style="margin-bottom: 0">
                        <div class="profile_option">
                            <span id="icon" class="material-symbols-outlined">
                                logout
                            </span>
                            <a href="#" id="submit-form-profile">Logout</a>
                        </div>
                        @csrf
                    </form>
                </div>
            </div>

            <div class="center-content">
                <div class="user-icon">
                    <div class="user" style="border-bottom: 1px solid lightgrey;">
                        <i class="fas fa-user-circle" style="color: white; margin: 25px 15px"></i>
                        <h4 style="font-weight: bold; color: white;">{{Auth::user()->name}}</h4>
                    </div>

                    <div class="menu">
                        <div class="option">
                            <span id="icon" class="material-symbols-outlined">
                                person
                            </span>
                            <a href="{{route('getUserProfile')}}">Account Detail</a>
                        </div>

                        <div class="option">
                            <span id="icon" class="material-symbols-outlined">
                                shopping_cart
                            </span>
                            <a href="{{ route('getOrderConfirm', ['id' => Auth()->user()->id]) }}">Order</a>
                        </div>
                        <div class="option">
                            <span id="icon" class="material-symbols-outlined">
                                manage_accounts
                            </span>
                            <a href="{{route('changePassword')}}">Change Password</a>
                        </div>

                        <form id="logout" action="{{route('logout')}}" method="POST" style="margin-bottom: 0">
                            <div class="option">
                                <span id="icon" class="material-symbols-outlined">
                                    logout
                                </span>
                                <a id="submit-form" href="#">Logout</a>
                            </div>
                            @csrf
                        </form>
                    </div>
                </div>
                <div class="info">
                    @yield('profile-content')
                </div>
            </div>
        </div>
    </div>
    <script>
        document.getElementById('submit-form').addEventListener('click', function(e) {
            e.preventDefault();
            document.getElementById('logout').submit();
        });

        document.getElementById('submit-form-profile').addEventListener('click', function(e) {
            e.preventDefault();
            document.getElementById('logout').submit();
        });
    </script>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function() {
            var menuVisible = false;

            $('.menu__profile__toggle').click(function() {
                if (menuVisible) {
                    $('.menu-profile').animate({ left: '-500px' });
                } else {
                    $('.menu-profile').animate({ left: '0px' });
                }
                menuVisible = !menuVisible;
            });

            $(document).click(function(event) {
                if (menuVisible && !$(event.target).closest('.menu-profile').length && !$(event.target).closest('.menu__profile__toggle').length) {
                    $('.menu-profile').animate({ left: '-500px' });
                    menuVisible = false;
                }
            });
        });
    </script>
@endsection
<style>
    .path {
        margin: 20px 0 15px 10% ;
    }

    .row {
        display: flex;
        justify-content: center;
    }

    .center-content {
        width: 85%;
        text-align: center;
        align-content: center;
        background: white;
        border-radius: 15px;
        box-shadow: 0px 5px 10px 5px lightgrey;
    }

    .user-icon {
        background: linear-gradient(to bottom left, #cc3300 32%, #ff0000 100%);
        width: 30%;
        height: 100%;
        float: left;
        margin-left: auto;
        border-top-left-radius: 15px;
        border-bottom-left-radius: 15px;
    }
    .user {
        /* height: 428px; */
    }
    .user-icon .menu {
        float: left;
        text-align: left;
        width: 100%;
        height: 428px;
        position: relative;
    }

    .menu .option {
        float: left;
        width: 100%;
        height: 15%;
        display: flex;
        align-items: center;
    }

    #logout-form {
        float: left;
        width: 100%;
        display: flex;
        align-items: center;
        margin: 0;
    }

    #icon {
        margin: 20px 15px 20px 25px;
        font-size: 32px;
        color: white;
    }

    .option:hover #icon {
        color: black;
    }

    .option a {
        margin: 10px;
        font-size: 18px;
        font-weight: bold;
        color: white;
        align-items: center;
        justify-content: center;
    }

    .option:hover a {
        color: black;
    }

    .option:hover {
        background: #ffcc99;
        transition: 0.5s;
    }

    .info {
        float: left;
        margin: 10px 0 10px 20px;
        width: 67%;
    }

    .path a {
        margin-right: 10px;
    }

    .path i {
        margin-right: 10px;
    }

    .user i {
        font-size: 80px;
    }
    .menu-profile {
        display: none;
    }
    .menu__profile__toggle {
        display: none;
    }

    @media (max-width:1140px){
        .user {
            height: 170px;;
        }
    }

    @media (max-width:830px){
        .option a {
            font-size: 15px;
        }

        .option #icon{
            font-size: 30px;
        }

        .row .menu-profile {
            display:block;
            left: -500px;
            box-shadow: 5px 0px 5px lightgrey;
            position: fixed;
            transition: top 0.3s ease;
        }
    }

    @media (max-width:760px){
        .user i {
            font-size: 60px;
        }
        .user h4{
            font-size: 20px;
        }

        .user-icon {
            display: none;
        }

        .menu__profile__toggle {
            display: block;
            position: fixed;
            margin: 5px 10px 0 0;
            cursor: pointer;
            padding: 5px 8px 5px 8px;
        }

        .info {
            float: left;
            margin: 10px 0 10px 10px;
            width: 95%;
        }

        .profile_option {
            display: flex;
            align-items: center;
        }

        .profile_option:hover {
            background-color: lightgray;
            cursor: pointer;
        }

        #option-logout .profile_option {
            border-radius: 0 0 15px 0;
        }

        .profile_option a {
            color:black;
        }

        .profile_option a:hover {
            color:black;
        }

        .profile_option #icon {
            color:black;
        }
    }

    @media (max-width: 395px) {
        .path {
            margin: 20px;
        }
    }
</style>

