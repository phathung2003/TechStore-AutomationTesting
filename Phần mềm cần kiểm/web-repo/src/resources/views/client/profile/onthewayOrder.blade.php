@extends('client.profile.order')
@section('order-content')
@if (!empty($order) && count($order) > 0)
        @foreach ($order as $key)
             @if(!empty(getOrderOntheway($key->id,$key->status)))
                        <div class="row d-flex justify-content-center align-items-center">
                          <div class="card" style="border-radius: 10px;margin-bottom:20px;border-color: #cacaca;box-shadow: 2px 2px 3px 3px #cacaca;">
                            <div class="card-body pb-2  ">
                              <div class="d-flex justify-content-between align-items-center mb-1">
                                <p class="fw-normal mb-0" style="color: #a8729a;">Name</p>
                                <p class="fw-normal mb-0" style="">{{$key->name}}</p>
                              </div>
                              <div class="d-flex justify-content-between align-items-center mb-1">
                                <p class="fw-normal mb-0" style="color: #a8729a;">Address</p>
                                <p class="fw-normal mb-0" style="">{{$key->address}}</p>
                              </div>
                              <div class="d-flex justify-content-between align-items-center mb-1">
                                <p class="fw-normal mb-0" style="color: #a8729a;">Phone</p>
                                <p class="fw-normal mb-0" style="">{{$key->phone}}</p>
                              </div>
                                  <div class="card shadow-0 border mb-4">
                                    <div class="card-body">
                                      <div class="row">
                                        <table class="table table-bordered" style="margin-top: 20px;">
                                          <thead>
                                            <tr>
                                              <th style="">Images</th>
                                              <th style="">Name</th>
                                              <th style="">Storage</th>
                                              <th style="">Color</th>
                                              <th style="">Quantity</th>
                                              <th style="">Price</th>
                                            </tr>
                                          </thead>
                                          <tbody>

                                                @foreach (getOrderOntheway($key->id,$key->status) as $item)
                                                  <tr>
                                                    <td><img src="{{asset(getImageProduct($item->product_id))}}" alt="" style="width: 100px; height:auto"></td>
                                                    <td>{{getNameProduct($item->product_id)}}</td>
                                                    <td>{{getStorage($item->product_id)}}</td>
                                                    <td>{{getColor($item->product_id)}}</td>
                                                    <td>{{$item->amount}}</td>
                                                    <td>{{$item->price}}</td>
                                                  </tr>
                                                  @endforeach

                                          </tbody>
                                        </table>
                                      </div>
                                      <hr class="mb-4" style="background-color: #e0e0e0; opacity: 1;">
                                    </div>
                                  </div>
                                  <div class="d-flex justify-content-between pt-1">
                                    <p class="fw-bold mb-0">Total</p>
                                    <p class="text-muted mb-0"><span class="fw-bold me-4"></span>{{laygiatienbandauDetail($key->id)}}</p>
                                  </div>
                            <div class="d-flex justify-content-between pt-1">
                              <p class="text-muted mb-0">Voucher</p>
                              <p class="text-muted mb-0"><span class="fw-bold me-4"></span> {{getDiscountPrice($key->discount_id)}}</p>
                            </div>
                            <div class="d-flex justify-content-between pt-1">
                              <p class="text-muted mb-0">Discount rank</p>
                              <p class="text-muted mb-0"><span class="fw-bold me-4">{{getPriceRank($key->id, $key->total,$key->discount_id)}}</span></p>
                            </div>
                          </div>
                          <div class="card-footer border-0 " style="background-color:white;">
                            <h5 class="d-flex align-items-center justify-content-end mb-0" style="font-weight: bold; color:red">Total paid: {{$key->total}}
                            </h5>
                          </div>
                      </div>

          @endif
        @endforeach
    @else
        <table>
            <tbody >
                <tr>
                <td colspan="5" style="text-align: center; vertical-align: middle; align-items:center; ">
                    <h5 style="display:block text-align:center ">No order has been made yet</h5>
                </td>
                </tr>
            </tbody>
        </table>
    @endif
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
