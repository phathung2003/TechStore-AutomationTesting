@extends('admin.layouts.master')

@section('title', 'Categories')

@section('content')
<div class="card shadow mb-4">
    <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">List of Categories</h6>
    </div>
    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th width="20%">Created_At</th>
                        <th width="20%">Updated_At</th>
                        <th width ="2%"></th>
                        <th width ="2%"></th>

                    </tr>
                </thead>
                <tbody>
                    @if (!empty($list))
                        @if(count($list) > 0)
                            @foreach ($list as $item)
                                <tr>
                                    <td>{{$item -> name}}</td>
                                    <td>{{$item -> description}}</td>
                                    <td>{{$item -> created_at}}</td>
                                    <td>{{$item -> updated_at}}</td>
                                    <td><a href="{{route('admin.categories.getUpdate',['id'=>$item ->id])}}" class="btn btn-warning">Sửa</a></td>
                                    <td><a href="{{route('admin.categories.delete',['id'=>$item ->id])}}" class="btn btn-danger">Xóa</a></td>
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
