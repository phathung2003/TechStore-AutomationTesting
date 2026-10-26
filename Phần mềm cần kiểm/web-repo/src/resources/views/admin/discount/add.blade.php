@extends('admin.layouts.master')

@section('title', 'Add a new discounts')

@section('content')

    <div class="card shadow mb-4">
        <div class="card-body">
            <form action="" method="POST">
                    <div class="mb-3">
                        <label for="">Name</label>
                        <input type="text" name="name" class="form-control w-50" placeholder="Discount Name..." value="{{old('name')}}">
                        @error('name')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Code</label>
                        <input type="text" name="code" class="form-control w-50" placeholder="Code..." value="{{old('code')}}">
                        @error('code')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Price</label>
                        <input type="text" name="price" class="form-control w-50" placeholder="Price..." value="{{old('price')}}">
                        @error('price')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                       
                                <label for="">Rank</label>
                                    <select name="rank" class= "form-control w-50">
                                        <option value="0">Open this select menu</option>
                                        @if(!empty($rank))
                                            @foreach($rank as $item)
                                            <option value="{{$item->id}}">{{$item->name}}</option>
                                            @endforeach
                                        @endif
                                    </select>
                                {{-- <input type="text" name="rank" class="form-control w-50" placeholder="Rank..." value="{{old('rank')}}"> --}}
                                @error('rank')
                                    <span style="color: red">{{$message}}</span>
                                @enderror      
                    </div>
                    <div class="mb-3">
                        <label for="">Amount</label>
                        <input type="text" name="amount" class="form-control w-50" placeholder="Amount..." value="{{old('amount')}}">
                        @error('amount')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Date start</label>
                        <input type="text" name="date_start" class="form-control w-50" placeholder="Date start..." value="{{old('date_start')}}">
                        @error('date_start')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Date end</label>
                        <input type="text" name="date_end" class="form-control w-50" placeholder="Date end..." value="{{old('date_end')}}">
                        @error('date_end')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                <button type="submit" class="btn btn-primary">Add New</button>
                @csrf
            </form>
        </div>
    </div>
    <script src="{{asset('admins')}}/vendor/jquery/jquery.min.js"></script>
    <script src="{{asset('admins')}}/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="{{asset('admins')}}/vendor/jquery-easing/jquery.easing.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="{{asset('admins')}}/js/sb-admin-2.min.js"></script>

    <!-- Page level plugins -->
    <script src="{{asset('admins')}}/vendor/chart.js/Chart.min.js"></script>

    <!-- Page level custom scripts -->
    <script src="{{asset('admins')}}/js/demo/chart-area-demo.js"></script>
    <script src="{{asset('admins')}}/js/demo/chart-pie-demo.js"></script>
@endsection
