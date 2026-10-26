@extends('admin.layouts.master')

@section('title', 'Add a new product')


@section('content')

    <div class="card shadow mb-4">
        <div class="card-body">
            <form action="{{route('admin.products.postAdd')}}" method="POST" enctype="multipart/form-data">
                <div style="display: grid; grid-template-columns: repeat(2, 1fr)">
                    <div class="mb-3">
                        <label for="">Code</label>
                        <input type="text" name="code" class="form-control w-50" placeholder="Product Code..." value="{{old('code')}}">
                        @error('code')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Category</label>
                        <select class="form-control w-50" required name="category">
                            <option value="0">Open this select menu</option>
                            @if (!empty(getAllCategory()))
                                @foreach(getAllCategory() as $item)
                                    <option value="{{$item->id}}">{{$item->name}}</option>
                                @endforeach
                            @endif
                        </select>
                        @error('category')
                          <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>


                    <div class="mb-3">
                        <label for="">Name</label>
                        <input type="text" name="name" class="form-control w-50" placeholder="Product Name..." value="{{old('name')}}">
                        @error('name')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Brand</label>
                        <select class="form-control w-50" required name="brand">
                            <option value="0">Open this select menu</option>
                            @if (!empty(getAllBrand()))
                                @foreach(getAllBrand() as $item)
                                    <option value="{{$item->id}}">{{$item->name}}</option>
                                @endforeach
                            @endif
                          </select>
                        @error('brand')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>

                    <div class="mb-3">
                        <label for="">Amount</label>
                        <input type="number" name="amount" class="form-control w-50" placeholder="Product Amount" min="1" max="1000" value="{{old('amount')}}">
                        @error('amount')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Buying Prices</label>
                        <input type="number" name="price_buy" class="form-control w-50" placeholder="Product Buying..." value="{{old('price_buy')}}">
                        @error('price_buy')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Image</label>
                        <input type="file" name="image" class="form-control w-50" placeholder="Product Imgae..." value="{{old('image')}}">
                        @error('image')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>

                    <div class="mb-3">
                        <label for="">Selling Prices</label>
                        <input type="number" name="price_sell" class="form-control w-50" placeholder="Product Selling..." value="{{old('price_sell')}}">
                        @error('price_sell')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Colors</label>
                        <input type="color" name="color" class="form-control w-50" placeholder="Product Selling..." value="{{old('sell')}}">
                        @error('color')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Storages</label>
                        <input type="number" name="storage" class="form-control w-50" placeholder="Product Storages..." value="{{old('storage')}}">
                        @error('storage')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>

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

