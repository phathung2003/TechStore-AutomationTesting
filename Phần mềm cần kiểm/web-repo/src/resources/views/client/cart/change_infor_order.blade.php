@extends('client.layouts.master')

@section('content')
<style>
</style>
    <div class="container">
        <div class="card shadow mb-4">
            <div class="card-body">
                <h4>Change Information Order</h4>
                <form action="" method="POST">
                        <div class="mb-1">
                            <label for="">Name</label>
                        </div>
                        <div class="mb-3">
                            <input type="text" name="nickName" class="form-control" placeholder="name..." autocomplete="off"  value="{{old('nickName') ?? Auth()->user()->name}}">
                            @error('nickName')
                                <span style="color: red">{{$message}}</span>
                            @enderror
                        </div>

                        <div class="mb-1">
                            <label for="">Phone</label>
                        </div>
                        <div class="mb-3">
                            <input type="text" name="phone" class="form-control" placeholder="Your Phone..." autocomplete="off"  value="{{old('phone')}}">
                            @error('phone')
                                <span style="color: red">{{$message}}</span>
                            @enderror
                        </div>

                        <div class="mb-1">
                            <label for="">Address</label>
                        </div>
                        <div class="mb-3">
                            <input type="text" name="address" class="form-control" placeholder="Your Addresss..." autocomplete="off" value="{{old('address')}}">
                            @error('address')
                                <span style="color: red">{{$message}}</span>
                            @enderror
                        </div>

                    <button type="submit" class="btn btn-primary">Submit</button>
                    @csrf
                </form>
            </div>
        </div>
    </div>
@endsection
