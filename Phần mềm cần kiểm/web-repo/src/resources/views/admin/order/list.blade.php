@extends('admin.layouts.master')

@section('title', 'Orders')

@section('content')
<div class="card shadow mb-4">
    <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">List of Orders</h6>
        @if (session('msg'))
            <div class="alert alert-success">{{session('msg')}}</div>
        @endif
        </h6>
    </div>
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                    <tr>
                        <th width="15%">Name</th>
                        <th width="15%">Total</th>
                        <th width="10%">Phone</th>
                        <th width ="20%">Address</th>
                        <th width ="15%">Voucher</th>
                        <th width ="10%">Value</th>
                        <th class="" width ="35%">Status</th>
                        <th width ="15%">Detail</th>
                        <th width ="15%">Update</th>
                    </tr>
                </thead>
                <tbody>
                    @if (!empty($list))
                        @if(count($list) > 0)
                            @foreach ($list as $item)
                                <tr>
                                    <td>{{$item->name ?? getNameUser($item->user_id)}}</td>
                                    <td>{{$item->total}}</td>
                                    <td>{{$item->phone}}</td>
                                    <td>{{$item->address}}</td>
                                    <td>{{getDiscountName($item->discount_id)}}</td>
                                    <td>{{getDiscountPrice($item->discount_id)}}</td>
                                    <td class=""><a href="#" class="btn btn-{{displayClassStatusOrder($item->status)}}">{{displayStatus($item->status)}}</td>
                                    <td><a href="{{route('admin.orders.detailOrder',['id'=>$item ->id])}}" class="btn btn-warning">Detail</a></td>
                                    <td><a href="{{route('admin.orders.getUpdateStatusOrder',['id'=>$item->id])}}" class="btn btn-primary">Update</a></td>
                                </tr>
                            @endforeach
                        @else
                            <td colspan="6">Data is not available</td>
                        @endif
                    @else
                        <td colspan="6">Server error responses</td>
                    @endif
                </tbody>
            </table>
        </div>
    </div>
</div>
<!-- Bootstrap core JavaScript-->
<script src="{{asset ('admins')}}/vendor/jquery/jquery.min.js"></script>
<script src="{{asset ('admins')}}/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

<!-- Core plugin JavaScript-->
<script src="{{asset ('admins')}}/vendor/jquery-easing/jquery.easing.min.js"></script>

<!-- Custom scripts for all pages-->
<script src="{{asset ('admins')}}/js/sb-admin-2.min.js"></script>

<!-- Page level plugins -->
<script src="{{asset ('admins')}}/vendor/datatables/jquery.dataTables.min.js"></script>
<script src="{{asset ('admins')}}/vendor/datatables/dataTables.bootstrap4.min.js"></script>

<!-- Page level custom scripts -->
<script src="{{asset ('admins')}}/js/demo/datatables-demo.js"></script>


@endsection
