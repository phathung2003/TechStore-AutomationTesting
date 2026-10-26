@extends('client.profile.master-profile')
@section('profile-content')
  <div class="row" style="display:flex; justify-content:center;">
    <div class="navbar navbar-light  rounded">
      <form class="form-inline">
        <a href="{{ route('getOrderConfirm', ['id' => Auth()->user()->id]) }}" class="btn btn-outline-success mr-3 ml-0" type="button">
          Pending
          {{-- <div class="icon">
            <span class="material-symbols-outlined">
                order
            </span>
            @if (!empty(Session::has('Cart')))
                <span id="icon-amount-orders">{{Session::get('Cart')->totalQuantity}}</span>
            @else
                <span id="icon-amount-orders">0</span>
            @endif
            <span class="tooltiptext">Cart</span>
        </div> --}}
        </a>
        <a href="{{ route('getOrderShipped', ['id' => Auth()->user()->id]) }}" class="btn btn-outline-success mr-3 ml-3" type="button">Processing</a>
        <a href="{{ route('getOrderOntheway', ['id' => Auth()->user()->id]) }}" class="btn btn-outline-success mr-3 ml-3" type="button">On the way</a>
        <a href="{{ route('getOrderDelivered', ['id' => Auth()->user()->id]) }}" class="btn btn-outline-success mr-3 ml-3" type="button">In Transit</a>
        <a href="{{ route('getOrdered', ['id' => Auth()->user()->id]) }}" class="btn btn-outline-success mr-0 ml-3" type="button">Cancelled</a>
        {{-- <button class="btn btn-sm btn-outline-secondary" type="button">Smaller button</button> --}}
      </form>
    </div>
    <div class="container mt-2">
      @yield('order-content')
  </div>
@endsection
<style>
    .table-order {
        width: 95%;
    }

    th {
        text-align: center;
        vertical-align: middle;
        align-items:center;
        padding: auto;
    }

    @media (max-width: 567px) {
        th {
            font-size: 12px;
        }
    }

    @media (max-width: 391px) {
        .table-order {
            width: 90%;
            margin-left: 0;
        }

        .table {
            width: 95%;
        }
    }
</style>
<script>
  //  const btnLoginForm1 = document.querySelector("#init-login-form-1");
  //       btnLoginForm1.addEventListener("click", changeStatus() );
</script>
