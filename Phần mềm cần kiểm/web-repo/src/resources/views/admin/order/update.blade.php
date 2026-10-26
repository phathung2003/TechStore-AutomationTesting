@extends('admin.layouts.master')

@section('title', 'Update Order')

@section('content')

    <div class="card shadow mb-4">
        <div class="card-body">
            @if(!empty($order))
            <form action="" method="POST">
                    <div class="mb-3">
                        <label for="">Code</label>
                        <input type="text" name="code" class="form-control w-50" placeholder="Code..." value="{{$order->id ?? old('code')}}" readonly>
                        @error('code')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Total</label>
                        <input type="text" name="total" class="form-control w-50" placeholder="Total..." value="{{$order->total ?? old('total')}}" readonly>
                        @error('total')
                            <span style="color: red">{{$message}}</span>
                        @enderror
                    </div>
                    <div class="mb-3">
                        <label for="">Status</label>
                            <select name="status" class="form-control w-50">
                                <option value="{{$order->status}}">{{displayStatus($order->status)}}</option>
                                @if($order->status != 2)
                                    @for ($i = ++$order->status; $i < 6; $i++)
                                        <option value="{{$i}}">{{displayStatus($i)}}</option>
                                    @endfor
                                @endif
                            </select>
                        @error('rank')
                            <span style="color: red">{{$message}}</span>
                        @enderror      
                    </div>
                <button type="submit" class="btn btn-primary">Update</button>
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
