@extends('admin.layouts.master')
@section('title', 'Update Brand')
@section('content')
    <div class="card shadow mb-4">
        <div class="card-body">
            @if(!empty($category))
                <form action="" method="POST">
                    <div class="mb-3">
                        <label for="brand_name">Name</label>
                        <input class="form-control" type="text" name="name" placeholder="Category name ..." value="{{old("name") ?? $category->name}}">
                        @error('name')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Description</label>
                        <input class="form-control" type="text" name="description" placeholder="Description ..." value="{{old("description") ?? $category->description}}">
                        @error('description')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <a href="{{route('admin.categories.getList')}}" class="btn btn-warning">Back</a>
                    <button class="btn btn-primary" type="submit">Update</button>
                    @csrf
                </form>
            @else
                <h2>Data is not available</h2>
            @endif

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
