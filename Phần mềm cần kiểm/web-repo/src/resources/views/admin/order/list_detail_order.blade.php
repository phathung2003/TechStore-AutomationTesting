@extends('admin.layouts.master')

@section('title', 'Products')

@section('content')
<div class="card shadow mb-4">
    <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">List of Products</h6>
    </div>
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                    <tr>
                        <th>Order Id</th>
                        <th width="15%">Name</th>
                        <th width="10%">Image</th>
                        <th>Amount</th>
                        <th>Price</th>

                    </tr>
                </thead>
                <tbody>
                    @if (!empty($orderDetailList))
                            @if(count($orderDetailList) > 0)
                                @foreach ($orderDetailList as $item)
                                    <tr>
                                        <td>{{$item->order_id}}</td>
                                        <td>{{getNameProduct($item->product_id)}}</td>
                                        <td><img src="{{asset(getImageProduct($item->product_id))}}" alt="" style="width: 100px; height:auto"></td>
                                        <td>{{$item->amount}}</td>
                                        <td>{{$item->price}}</td>

                                    </tr>
                                @endforeach
                            @else
                                <td colspan="12">Data is not available</td>
                            @endif
                        @else
                            <td colspan="12">Server error responses</td>
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

<script>
    let son = document.querySelector("#hi");
    son.classList.remove("sorting");
    console.log(son);
</script>
@endsection
