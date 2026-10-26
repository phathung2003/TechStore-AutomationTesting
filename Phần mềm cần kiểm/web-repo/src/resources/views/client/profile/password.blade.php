@extends('client.profile.master-profile')
@section('profile-content')
<div class="row" style="display:flex; justify-content:center">
    <div style="width:95%">
        <h2 style="float: left; margin-top: 5%">Change Password</h2>
    </div>
    <div class="password" style="width:95%">
        <fieldset id="change">
            <form action="{{route('postChangePassword')}}" method="POST" enctype="multipart/form-data" style="margin-left: 20px;">
                <div class="mb-4">
                    <label>Current Password:</label>
                    <input type="password" name="currentPassword" class="form-control" placeholder="Type your current password...">
                    @error('currentPassword')
                        <span>{{$message}}</span>
                    @enderror
                </div>

                <div class="mb-4">
                    <label>New Password:</label>
                    <input type="password" name="newPassword" class="form-control" placeholder="Type your new password...">
                    @error('newPassword')
                        <span>{{$message}}</span>
                    @enderror
                </div>

                <div class="mb-4">
                    <label>Confirm New Password:</label>
                    <input type="password" name="confirmPassword" class="form-control" placeholder="Type your confirm password...">
                    @error('confirmPassword')
                        <span>{{$message}}</span>
                    @enderror
                </div>

                @if (session('success'))
                    <div class="alert alert-success">
                        {{ session('success') }}
                    </div>
                @endif

                @if (session('error'))
                    <div class="alert alert-danger">
                        {{ session('error') }}
                    </div>
                @endif

                @csrf
                <button id="btn-save" type="submit" class="btn btn-primary" style="float: left; margin: 10px 0 8px 0">Save Change</button>
            </form>
        </fieldset>
    </div>
</div>

@endsection
<style>

    fieldset label {
        float: left;
        margin-top: 15px;
    }

    fieldset .mb-4 {
        width: 95%;
    }

    fieldset span {
        color: red;
        float: left;
        margin-top: 5px
    }

    fieldset {
        margin-right: 10px !important;
        border: 2px solid lightgrey !important;
        border-radius: 10px;
        padding:10px !important;
        width: 95% !important;
    }

    legend {
        float: none;
        max-width: 35% !important;
        text-align: left !important;
        padding: 0px 20px 0 !important;
        font-size: 20px !important;
    }

    @media (max-width:1140px){
        .user {
            height: 28%;;
        }
    }

    @media (max-width: 760px){
        fieldset {
            margin-right: 7px !important;
            padding:5px !important;
            width: 98% !important;
        }
    }

    @media (max-width: 391px){
        fieldset {
            width: 93% !important;
            margin-right: 10px !important;
        }
    }

</style>
