@extends('client.layouts.master')
@section('content')
    <div class="container" style="max-width:1270px">
        @if (true)
        <div class="main-title">
            <h2>Shopping Cart</h2>
        </div>
            <div id="main-cart">
                @if (!empty(Session::has("Cart")))
                    <div id="content-left">
                        <div class="content-left-head ">
                            <label for="" style="margin-bottom: 0;">Tất cả sản phẩm</label>
                            <span class="ta-c">Đơn giá</span>
                            <span class="ta-c">Số lượng</span>
                            <span class="ta-c">Thành tiền</span>
                            <span><i class="fa-regular fa-trash-can fa-lg"></i></span>
                        </div>
                        <div class="content-left-main">
                            @foreach (Session::get("Cart")->products as $item)
                                <div class="test r-item" id="wrap-item-{{$item['productInfo']->id}}">
                                    <div class="">
                                        <div class="product-info">
                                            <a href="" class="link-img">
                                                <div class="block-product-img">
                                                    <img src="{{asset($item['productInfo']->images)}}" alt="" class="product-img">
                                                </div>
                                            </a>
                                            <div class="product-name">
                                                <h5 style="margin-bottom: 0">{{$item['productInfo']->name}}</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="ta-c">
                                        <span >{{$item['productInfo']->price_sell}}</span>
                                    </div>
                                    <div class="">
                                        <div class="qty-container">
                                            <button class="qty-btn-minus btn-light" type="button" onclick="decreaseItem({{$item['productInfo']->id}})"><i class="fa fa-minus"></i></button>
                                            <input type="text" name="qty" value="{{$item['quantity']}}" class="input-qty" id="item-cart-{{$item['productInfo']->id}}" max="{{$item['productInfo']->amount}}"/>
                                            <button class="qty-btn-plus btn-light" type="button" onclick="increaseItem({{$item['productInfo']->id}}, {{$item['productInfo']->amount}})"><i class="fa fa-plus" ></i></button>
                                        </div>
                                    </div>
                                    <div class="ta-c" id="item-price-{{$item['productInfo']->id}}">{{$item['price']}}</div>
                                    <div class="" onclick="deleteItemListCart({{$item['productInfo']->id}})" style="cursor: pointer">
                                        <span class="material-symbols-outlined">
                                            delete
                                        </span>
                                    </div>
                                </div>
                            @endforeach
                        </div>
                    </div>
                    <div id="content-right">
                        <div class="block-info-customer">
                            <div class="content-bic">
                                <div class="form-change-info"></div>
                                <div class="head-content">
                                    <h3 class="hcb-title">Giao tới</h3>
                                    <a href="{{route('getChangeInfoOrder')}}" class="change-address">Change</a>
                                </div>
                                @if ((Session('Cart')->nickName && Session('Cart')->phone && Session('Cart')->address) || (Auth::user()->phone && Auth::user()->address))
                                    <div class="customer-info">
                                        <p class="customer-info-name">{{Session('Cart')->nickName ?? Auth::user()->name }}</p>
                                        <i style="display: block;width: 1px;height: 20px;background-color: rgb(235, 235, 240); margin: 0px 8px;"></i>
                                        <p class="customer-info-phone">{{Session('Cart')->phone ?? Auth::user()->phone }}</p>
                                    </div>
                                    <div class="address">
                                        <span style="color: rgb(0, 171, 86);
                                        background-color: rgb(239, 255, 244);
                                        font-weight: 500;
                                        font-size: 12px;
                                        line-height: 16px;
                                        padding: 0px 6px;
                                        border-radius: 100px;
                                        margin-right: 4px;
                                        height: 18px;
                                        display: inline-flex;
                                        -webkit-box-align: center;
                                        align-items: center;">Home</span>
                                        {{Session('Cart')->address ?? Auth::user()->address }}
                                    </div>
                                @else
                                <div class="customer-info">
                                    <p class="customer-info-name">{{Auth::user()->name }}</p>
                                </div>
                                    <div class="address">
                                        <span style="color: #d70018">Please change the shipping information because of its NULL</span>
                                    </div>
                                @endif

                            </div>
                        </div>
                        <div class="block-voucher">
                            <div class="content-bv">
                                <div class="head-content">
                                    <h3 class="hcb-title" style="color: rgb(36, 36, 36);">Shop khuyến mãi</h3>
                                    <h4 class="hcb-title" style="font-size: 14px">Tối đa chọn 1</h4>
                                </div>
                                    @if(Session::get('discount'))
                                        @foreach(Session::get('discount') as $key=>$item)
                                            @if(!empty($item['price']))
                                                <form method="POST" action="{{route('checkDiscount')}}">
                                                    <div class="head-content">
                                                    <input type="text" name="voucher" placeholder="Nhập mã khuyến mãi..." class="customer-voucher form-control" value="{{$item['code']}}" style="width: 240px; margin-right:4px" >
                                                    <button type="submit" class="btn btn-primary">Add</button>
                                                    </div>
                                                    @csrf
                                                </form>
                                            @endif
                                        @endforeach
                                    @else
                                        <form method="POST" action="{{route('checkDiscount')}}">
                                            <div class="head-content">
                                            <input type="text" name="voucher" placeholder="Nhập mã khuyến mãi..." class="customer-voucher form-control" style="width: 240px; margin-right:4px" >
                                            <button type="submit" class="btn btn-primary">Add</button>
                                            </div>
                                            @csrf
                                        </form>
                                    @endif
                                <div class="head-content">
                                    <span id="voucher-link">Select Your Voucher</span>
                                </div>
                            </div>
                        </div>
                        <div class="block-checkout">
                            <ul class="price-items">
                                <li class="price-item">
                                    <span class="price-text">Tạm tính</span>
                                    <span class="price-value" id ="pre-total-price">{{Session::get('Cart') ? Session::get('Cart')->totalPrice : 0}}</span>
                                </li>
                                <li class="price-item">
                                    @if(!empty($nameRank))
                                        @if(!empty($discount))
                                            <span class="price-text">Hạng {{$nameRank}}</span>
                                            <span class="price-value" id ="price-discount-rank">-{{Session::get('Cart') ? ((Session::get('Cart')->totalPrice)*$discount)/100 : 0}}</span>
                                        @endif
                                    @else
                                        <span class="price-text">Not Rank</span>
                                    @endif
                                </li>
                                @if(Session::get('discount'))
                                    @foreach(Session::get('discount') as $key=>$item)
                                        @if(!empty($item['price']))
                                            <li class="price-item">
                                                <span class="price-text">Giảm giá</span>
                                                <div class="price-value">-{{$item['price'] ?? 0}}</div>
                                            </li>
                                        @endif
                                        @if(!empty($item['name']))
                                            <li class="price-item">
                                                <span class="price-text">Voucher</span>
                                                <div class="price-value">{{$item['name'] ?? 0}}</div>
                                            </li>
                                        @endif
                                    @endforeach
                                @else
                                    <li class="price-item">
                                        <span class="price-text">Giảm giá</span>
                                        <div class="price-value">0</div>
                                    </li>
                                @endif
                            </ul>
                            <div class="price-total">
                                <span class="price-text">Tổng tiền</span>
                                <div style="text-align:right">
                                    @if(!empty($discount))
                                        @if(Session::get('discount'))
                                            @foreach(Session::get('discount') as $key=>$item)
                                                    @if(!empty($item['price']))
                                                        <span id="price-total-value">{{(Session::get('Cart')->totalPrice - ((Session::get('Cart')->totalPrice *$discount)/100)) - $item['price'] ?? 0}}</span>
                                                    @else
                                                        <span id="price-total-value">{{(Session::get('Cart')->totalPrice - ((Session::get('Cart')->totalPrice *$discount)/100)) ?? 0}}</span>
                                                    @endif
                                            @endforeach
                                        @else
                                            <span id="price-total-value">{{Session::get('Cart') ? Session::get('Cart')->totalPrice - (((Session::get('Cart')->totalPrice)*$discount)/100) : 0}}</span>
                                        @endif
                                    @else
                                            @if(Session::get('discount'))
                                                @foreach(Session::get('discount') as $key=>$item)
                                                        @if(!empty($item['price']))
                                                            <span id="price-total-value">{{(Session::get('Cart')->totalPrice) - $item['price'] ?? 0}}</span>
                                                        @endif
                                                @endforeach
                                            @else
                                                <span id="price-total-value">{{Session::get('Cart') ? Session::get('Cart')->totalPrice : 0}}</span>
                                            @endif
                                    @endif
                                    <span class="notice-vat">(Đã bao gồm VAT nếu có)</span>
                                </div>
                            </div>
                        </div>
                        <a href="javascript:" onclick="checkOut()" class="btn-checkout">Checkout</a>
                        <form action="{{route('checkPayment')}}" method="POST" style="margin-top: 12px">
                            <input id="price-payment" name="totalPrice" type="hidden" value="{{Session::get('Cart') ? Session::get('Cart')->totalPrice : 0}}">
                            <button type="submit" class="btn btn-primary" name="redirect" style="width:100%">Check Payment</button>
                            @csrf
                        </form>
                    </div>
                    <div class="info_checkout">
                        <div style="position:fixed; width:97%; background-color:white; box-shadow: 5px 5px 10px 8px lightgrey; bottom:0">
                            <div style="display:flex; width:100%; border-bottom: 1px solid lightgrey;">
                                <div class="address_info" style="padding:16px 16px 0 16px;">
                                    <div class="head-content">
                                        <h3 class="hcb-title">Giao tới</h3>
                                        <a href="{{route('getChangeInfoOrder')}}" class="change-address">Change</a>
                                    </div>
                                    @if ((Session('Cart')->nickName && Session('Cart')->phone && Session('Cart')->address) || (Auth::user()->phone && Auth::user()->address))
                                        <div class="customer-info">
                                            <p class="customer-info-name">{{Session('Cart')->nickName ?? Auth::user()->name }}</p>
                                            <i style="width: 2px; height: 20px;background-color: rgb(235, 235, 240); margin: 0 8px;"></i>
                                            <p class="customer-info-phone">{{Session('Cart')->phone ?? Auth::user()->phone }}</p>
                                        </div>
                                        <div class="address">
                                            <span style="color: rgb(0, 171, 86);
                                                background-color: rgb(239, 255, 244);
                                                font-weight: 500;
                                                font-size: 10px;
                                                line-height: 16px;
                                                padding: 0px 5px;
                                                border-radius: 100px;
                                                height: 18px;
                                                display: inline-flex;
                                                -webkit-box-align: center;
                                                align-items: center;">
                                                Home
                                            </span>
                                            <span style="font-size: 14px">{{Session('Cart')->address ?? Auth::user()->address }}</span>
                                        </div>
                                    @else
                                        <div class="customer-info" style="margin-bottom: 8px">
                                            <p style="font-size:13px" class="customer-info-name">{{Auth::user()->name }}</p>
                                        </div>
                                        <div class="address" style="margin-bottom: 8px">
                                            <span style="color: #d70018; font-size:13px;">Please change the shipping information because of its NULL</span>
                                        </div>
                                    @endif
                                </div>

                                <div class="block-voucher-info" style="width:40%;">
                                    <div class="block-voucher">
                                        <div class="content-bv" style="margin:0; padding-bottom:0">
                                            <div class="head-content">
                                                <h3 class="hcb-title" style="color: rgb(36, 36, 36);">Shop khuyến mãi</h3>
                                                <h4 class="hcb-title" style="font-size: 14px">Tối đa chọn 1</h4>
                                            </div>
                                                @if(Session::get('discount'))
                                                    @foreach(Session::get('discount') as $key=>$item)
                                                        @if(!empty($item['price']))
                                                            <form method="POST" action="{{route('checkDiscount')}}">
                                                                <div class="head-content">
                                                                <input type="text" name="voucher" placeholder="Nhập mã khuyến mãi..." class="customer-voucher form-control" value="{{$item['code']}}" style="width: 240px;" >
                                                                <button type="submit" class="btn btn-primary">Add</button>
                                                                </div>
                                                                @csrf
                                                            </form>
                                                        @endif
                                                    @endforeach
                                                @else
                                                    <form method="POST" action="{{route('checkDiscount')}}">
                                                        <div class="head-content">
                                                        <input type="text" name="voucher" placeholder="Nhập mã khuyến mãi..." class="customer-voucher form-control" style="width: 90%;" >
                                                        <button style="margin-left:5px" type="submit" class="btn btn-primary">Add</button>
                                                        </div>
                                                        @csrf
                                                    </form>
                                                @endif
                                            <div class="head-content">
                                                <span id="voucher-link-1" style="color: blue">Select Your Voucher</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="block-checkout-info">
                                    <ul class="price-items">
                                        <li class="price-item">
                                            <span class="price-text">Tạm tính</span>
                                            <span class="price-value" id ="pre-total-price-1">{{Session::get('Cart') ? Session::get('Cart')->totalPrice : 0}}</span>
                                        </li>
                                        <li class="price-item">
                                            @if(!empty($nameRank))
                                                @if(!empty($discount))
                                                    <span class="price-text">Hạng {{$nameRank}}</span>
                                                    <span class="price-value" id ="price-discount-rank-1">-{{Session::get('Cart') ? ((Session::get('Cart')->totalPrice)*$discount)/100 : 0}}</span>
                                                @endif
                                            @else
                                                <span class="price-text">Not Rank</span>
                                            @endif
                                        </li>
                                        @if(Session::get('discount'))
                                            @foreach(Session::get('discount') as $key=>$item)
                                                @if(!empty($item['price']))
                                                    <li class="price-item">
                                                        <span class="price-text">Giảm giá</span>
                                                        <div class="price-value">-{{$item['price'] ?? 0}}</div>
                                                    </li>
                                                @endif
                                                @if(!empty($item['name']))
                                                    <li class="price-item">
                                                        <span class="price-text">Voucher</span>
                                                        <div class="price-value">{{$item['name'] ?? 0}}</div>
                                                    </li>
                                                @endif
                                            @endforeach
                                        @else
                                            <li class="price-item">
                                                <span class="price-text">Giảm giá</span>
                                                <div class="price-value">0</div>
                                            </li>
                                        @endif
                                    </ul>
                                </div>
                            </div>
                            <div class="block-voucher-info-1" style="width:100%;">
                                <div class="block-voucher">
                                    <div class="content-bv" style="margin:0; padding: 10px 16px 0 16px; border-bottom: 1px solid lightgrey;">
                                        <div class="head-content" style="margin-bottom:4px">
                                            <h3 class="hcb-title" style="color: rgb(36, 36, 36);">Shop khuyến mãi</h3>
                                            <h4 class="hcb-title">Tối đa chọn 1</h4>
                                        </div>
                                            @if(Session::get('discount'))
                                                @foreach(Session::get('discount') as $key=>$item)
                                                    @if(!empty($item['price']))
                                                        <form method="POST" action="{{route('checkDiscount')}}">
                                                            <div class="head-content">
                                                            <input type="text" name="voucher" placeholder="Nhập mã khuyến mãi..." class="customer-voucher form-control" value="{{$item['code']}}" style="width: 240px;" >
                                                            <button type="submit" class="btn btn-primary">Add</button>
                                                            </div>
                                                            @csrf
                                                        </form>
                                                    @endif
                                                @endforeach
                                            @else
                                                <form method="POST" action="{{route('checkDiscount')}}">
                                                    <div class="head-content" style="margin-bottom:0">
                                                    <input type="text" name="voucher" placeholder="Nhập mã khuyến mãi..." class="customer-voucher form-control" style="width: 90%;" >
                                                    <button style="margin-left:5px" type="submit" class="btn btn-primary">Add</button>
                                                    </div>
                                                    @csrf
                                                </form>
                                            @endif
                                        <div class="head-content" style="margin: 4px 0 4px 0;">
                                            <span id="voucher-link-2" style="color: blue">Select Your Voucher</span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                                <div class="block-checkout-payment" style="align-items:center; width:100%;">
                                    <div style="text-align:right; width:145px">
                                        <div class="price-text" style="height:40%; display:flex; align-items:center; justify-content:right">
                                            <span style="margin-right:10%">Tổng tiền</span>
                                            <div>
                                                @if(!empty($discount))
                                                    @if(Session::get('discount'))
                                                        @foreach(Session::get('discount') as $key=>$item)
                                                                @if(!empty($item['price']))
                                                                    <span id="price-total-value-1">{{(Session::get('Cart')->totalPrice - ((Session::get('Cart')->totalPrice *$discount)/100)) - $item['price'] ?? 0}}</span>
                                                                @else
                                                                    <span id="price-total-value-1">{{(Session::get('Cart')->totalPrice - ((Session::get('Cart')->totalPrice *$discount)/100)) ?? 0}}</span>
                                                                @endif
                                                        @endforeach
                                                    @else
                                                        <span id="price-total-value-1">{{Session::get('Cart') ? Session::get('Cart')->totalPrice - (((Session::get('Cart')->totalPrice)*$discount)/100) : 0}}</span>
                                                    @endif
                                                @else
                                                        @if(Session::get('discount'))
                                                            @foreach(Session::get('discount') as $key=>$item)
                                                                    @if(!empty($item['price']))
                                                                        <span id="price-total-value-1">{{(Session::get('Cart')->totalPrice) - $item['price'] ?? 0}}</span>
                                                                    @endif
                                                            @endforeach
                                                        @else
                                                            <span id="price-total-value-1">{{Session::get('Cart') ? Session::get('Cart')->totalPrice : 0}}</span>
                                                        @endif
                                                @endif
                                            </div>
                                        </div>

                                        <div>
                                            <span class="notice-vat">(Đã bao gồm VAT nếu có)</span>
                                        </div>
                                    </div>
                                    <div class="checkout">
                                        <a href="javascript:" onclick="checkOut()" class="btn btn-danger" style="background-color: rgb(255, 66, 78);">Checkout</a>
                                    </div>
                                    <div class="payment">
                                        <form action="{{route('checkPayment')}}" method="POST">
                                            <button type="submit" class="btn btn-primary" name="redirect">Check Payment</button>
                                            @csrf
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                @else
                <h4 style="width:fit-content; color:#d70018;">Cart is null</h4>
                @endif
            </div>

    @else
        <h1>Hãy đăng nhập</h1>
    @endif
    </div>

    <script>
        function renderListCart(response, id){
            $("#item-cart-"+id).val(`${response[0].quantity}`);
            $("#item-price-"+id).text(response[0].price);
            $("#icon-amount-orders").text(response[1]);
            $("#price-total-value").text(response[3]);
            $("#price-total-value-1").text(response[3]);
            $("#pre-total-price").text(response[2]);
            $("#pre-total-price-1").text(response[2]);
            $("#price-discount-rank").text(response[4]);
            $("#price-discount-rank-1").text(response[4]);
            $("#price-payment").val(response[3]);
        }

        function decreaseItem(id) {
            var $n =  $("#item-cart-"+id)
            var amount = Number($n.val());
            if(amount > 1) {
                $n.val(amount-1);
                $.ajax({
                    url:'updateItemListCart/' +id+ '/' +$("#item-cart-"+id).val() ,
                    type:'GET',
                }).done(function(response){
                    renderListCart(response, id);
                });
            }

        }
        function increaseItem(id, amount){
            var $n = $("#item-cart-"+id);
            console.log(amount);
            if($n.val() < amount) {
                $n.val(Number($n.val())+1);
                $.ajax({
                    url:'updateItemListCart/' +id+ '/' +$("#item-cart-"+id).val() ,
                    type:'GET',
                }).done(function(response){
                    renderListCart(response, id);
                });
            }
        }

        function deleteItemListCart(id) {
            $.ajax({
                url:'deleteItemListCart/'+id,
                type:'GET',
            }).done(function(response){
                if(response == 0){
                    $("#content-left").html(`<h4 style="width:fit-content; color:#d70018;">Cart is null</h4>`);
                    $("#icon-amount-orders").text("0");
                    $("#price-total-value").text("0");
                    $("#pre-total-price").text("0");
                    $("#price-total-value-1").text("0");
                    $("#pre-total-price-1").text("0");
                    $("#price-discount-rank").text("0");
                    $("#price-discount-rank-1").text("0");
                } else {
                    $("#wrap-item-"+id).remove();
                    $("#icon-amount-orders").text(response[0]);
                    $("#price-total-value").text(response[1]);
                    $("#pre-total-price").text(response[1]);
                    $("#price-total-value-1").text(response[1]);
                    $("#pre-total-price-1").text(response[1]);
                }
                Swal.fire(
                    'Item of cart was deleted successful',
                    'You clicked the button!',
                    'success'
                    )
            });
        }
        function checkOut(){
            $.ajax({
                url:'cart/checkout',
                type:'GET',
            }).done(function(response){
                if(response.status === 'error') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: `${response.des}`,
                        showCancelButton: true,
                        confirmButtonText: '<a href ="{{route("getChangeInfoOrder")}}" style ="color:white">Change</a>',
                    })
                } else if (response.status === 'success') {
                    console.log(response);
                    $("#content-left").html(`<h4 style="width:fit-content; color:#d70018;">Cart is null</h4>`);
                    $("#content-right").html(``);
                    $("#icon-amount-orders").text("0");
                    $("#price-total-value").text("0");
                    $("#pre-total-price").text("0");
                    $("#price-total-value-1").text("0");
                    $("#pre-total-price-1").text("0");
                    Swal.fire(
                        'The order was checked out successful',
                        'You clicked the button!',
                        'success'
                    )
                }
            });
        }
            let voucherLink = document.getElementById('voucher-link');
            voucherLink.addEventListener('click', () => {
                let voucherWrap = document.getElementById('vouchers-wrap');
                let sontran = document.getElementById("sontran");
                voucherWrap.style.display = "block";
                voucherWrap.style.transform = "translate(-50%, -13%)";
                let closeVoucher = document.getElementById('close-vouchers-wrap');
                sontran.style.display = "block";
                closeVoucher.addEventListener('click', () => {
                    voucherWrap.style.display = "none";
                    sontran.style.display = "none";

                })
            })

            let voucherLink1 = document.getElementById('voucher-link-1');
            voucherLink1.addEventListener('click', () => {
                let voucherWrap = document.getElementById('vouchers-wrap');
                let sontran = document.getElementById("sontran");
                voucherWrap.style.display = "block";
                voucherWrap.style.transform = "translate(-50%, -13%)";
                let closeVoucher = document.getElementById('close-vouchers-wrap');
                sontran.style.display = "block";
                closeVoucher.addEventListener('click', () => {
                    voucherWrap.style.display = "none";
                    sontran.style.display = "none";
                })
            })

            let voucherLink2 = document.getElementById('voucher-link-2');
            voucherLink2.addEventListener('click', () => {
                let voucherWrap = document.getElementById('vouchers-wrap');
                let sontran = document.getElementById("sontran");
                voucherWrap.style.display = "block";
                voucherWrap.style.transform = "translate(-50%, -13%)";
                let closeVoucher = document.getElementById('close-vouchers-wrap');
                sontran.style.display = "block";
                closeVoucher.addEventListener('click', () => {
                    voucherWrap.style.display = "none";
                    sontran.style.display = "none";
                })
            })
        // function deleteAll(){
        //     $.ajax({
        //         url:'cart/checkOut',
        //         type:'GET',
        //     }).done(function(respone){
        //         renderListCart(respone);
        //         Swal.fire(
        //             'Bạn đã đặt hàng thành công',
        //             'You clicked the button!',
        //             'success'
        //             )
        //     });
        // }

    </script>
@endsection
<style>

    .test {
        display: grid;
        grid-template-columns: 358px 150px 120px 150px 30px;
        align-items: center;
    }
    #main-cart {
        display: flex;
        flex-wrap: nowrap;
        -webkit-box-pack: justify;
        justify-content: space-between;


    }
    .main-title {
        margin: 20px 0px;
        display: flex;
        -webkit-box-align: center;
        align-items: center;
    }
    .main-title h4 {
        font-size: 20px;
        font-weight: 500;
        margin: 0px;
        color: rgb(0, 0, 0);
        line-height: 28px;
        text-transform: uppercase;
        flex-basis: calc(797px);
    }
    #content-left{
        flex: 1 1 1200px;
    }
    #content-right {
        flex: 1 1 calc(100% - 1200px);
        margin-left: 20px;
        display: block;
    }
    .content-left-head {
        background: rgb(255, 255, 255);
        padding: 9px 16px;
        border-radius: 4px;
        color: rgb(36, 36, 36);
        font-weight: 400;
        font-size: 13px;
        margin-bottom: 12px;
        top: 105px;
        z-index: 99;
        display: grid;
        grid-template-columns: 358px 150px 120px 150px 30px;
        -webkit-box-align: center;
        align-items: center;
    }
    .r-item {
        margin-left: 0px;
        align-items: center;
        margin-bottom: 12px;
        background-color: white;
        padding: 12px 16px;
        width: 100%;

    }
    .ta-c {
        text-align: center;
    }
    form {
        margin: 0;
    }

    .product-info {
        display: flex;
        align-items: center;
    }
    .link-img {
        width: 25%;

    }
    .block-product-img {
        padding: 4px;

    }
    .product-img {
        width: 100%;
    }
    .product-name {
        margin-left: 20px;
    }
    .block-info-customer {
        background: white;
        border-radius: 4px;

    }
    .content-bic {
        border-radius: 4px;
        margin-bottom: 12px;
        padding: 16px;
        font-size: 14px;
        line-height: 20px;
    }
    .head-content {
        display: flex;
        -webkit-box-align: center;
        align-items: center;
        -webkit-box-pack: justify;
        justify-content: space-between;
        margin-bottom: 12px;
    }
    .customer-info {
        display: flex;
        align-items: center;
        color: rgb(56, 56, 61);
        margin-bottom: 2px;
        font-weight: 600;
    }
    .address {
        color: rgb(128, 128, 137);
        font-weight: normal;
    }
    .hcb-title {
        color: rgb(128, 128, 137);
        font-weight: normal;
        margin: 0px;
        font-size: 16px;
        line-height: 20px;
    }
    a.change-address:hover {
        text-decoration: none;
    }
    p.customer-info-name {
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 1;
        overflow: hidden;
        margin: 0px;
    }
    p.customer-info-phone {
        margin: 0px;
        flex-shrink: 0;
    }
    .customer-info i {
        display: block;
        width: 1px;
        height: 20px;
        background-color: rgb(235, 235, 240);
        margin: 0px 8px;
    }
    .block-voucher {
        background-color: white;
        border-radius: 4px;
    }
    .content-bv {
        border-radius: 4px;
        margin-bottom: 12px;
        padding: 16px;
        font-size: 14px;
        line-height: 20px;
    }
    .block-checkout {
        background: rgb(255, 255, 255);
        border-radius: 4px;
        padding-bottom: 8px;
    }
    .price-items {
        list-style: none;
        margin: 0px;
        padding: 17px 20px;
        border-bottom: 1px solid rgb(244, 244, 244);
    }
    .price-item {
        display: flex;
        flex-wrap: nowrap;
        -webkit-box-pack: justify;
        justify-content: space-between;
        margin-bottom: 10px;
    }
    .price-text {
        font-weight: 300;
        color: rgb(51, 51, 51);
        display: inline-block;
    }
    .price-value {

    }
    .price-total {
        padding: 17px 20px;
        display: flex;
        flex-wrap: nowrap;
        -webkit-box-pack: justify;
        justify-content: space-between;
        margin: 0px;
    }
    #price-total-value, #price-total-value-1, #price-total-value-2 {
        color: rgb(254, 56, 52);
        font-size: 24px;
        font-weight: 400;
        text-align: right;
    }
    .notice-vat {
        font-weight: 300;
        font-style: normal;
        display: block;
        font-size: 12px;
        color: rgb(51, 51, 51);
        margin-top: 3px;
    }
    .btn-checkout {
        background: rgb(255, 66, 78);
        color: rgb(255, 255, 255);
        padding: 13px 10px;
        text-align: center;
        border-radius: 4px;
        border: none;
        width: 100%;
        display: block;
        cursor: pointer;
        margin: 15px 0px 0px;
    }
    .btn-checkout:hover {
        text-decoration: none;
        opacity: 0.8;
        color: white;
    }
    .r-item .col-3 {
        display: flex;
        justify-content: center;
        align-items: center;
        user-select: none;
    }
    .minus, .plus {
        display: inline-block;
        width: 20px;
        height: 20px;
        background-color: #EA6A9E;
        color: #fff;
        text-align: center;
        cursor: pointer;
        }
    .num {
        padding: 0 10px;
    }
    .qty-container{
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .qty-container .input-qty{
        text-align: center;
        padding: 6px 10px;
        border: 1px solid #d4d4d4;
        max-width: 40px;
        height: 21.6px;
    }
    .qty-container .qty-btn-minus,
    .qty-container .qty-btn-plus{
        border: 1px solid #d4d4d4;
        padding: 10px 13px;
        font-size: 10px;
        height: 20px;
        width: 20px;
        transition: 0.3s;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .qty-container .qty-btn-plus{
        margin-left: -1px;
    }
    .qty-container .qty-btn-minus{
        margin-right: -1px;
    }

    .info_checkout {
        display: none;
    }

    @media (max-width: 1215px){
        #content-left {
            width: 70%;
        }

        #content-right {
            display: none;
        }

        .content-left-head {
            grid-template-columns: 35% 17% 24% 20% auto;
        }

        .test {
            grid-template-columns: 35% 17% 24% 20% auto;
        }

        .info_checkout {
            display: block;
            position: absolute;
            top:0
        }

        .block-voucher-info-1 {
            display: none;
        }

        .address_info {
            width: 30%;
        }

        .block-checkout-info {
            width: 30%;
        }

        .block-checkout-payment {
            display: flex;
            justify-content: flex-end;
            margin: 8px 0;
        }

        .price-items {
            border-bottom: none;
        }

        .block-checkout-payment .checkout{
            margin: 0 5px 0 8px;
        }

        .block-checkout-payment .payment{
            margin: 0 16px 0 5px;
        }
    }

    @media (max-width: 736px){

        .customer-info {
            display: block;
        }
        .customer-info i{
            display: none;
        }
        .customer-info p{
            margin-bottom: 5px;
        }
    }

    @media (max-width: 500px){
        .block-voucher-info {
            display: none;
        }
        .block-voucher-info-1 {
            display: block;
        }

        .address_info {
            width: 58%;
        }

        .block-checkout-info {
            width: 42%;
        }

        .customer-info p{
            display: inline;
        }

        .customer-info i{
            display: inline;
        }

        .price-items {
            padding: 16px 16px 0 16px;
        }
    }

    @media (max-width:431px){
        .hcb-title {
            font-size: 12px;
        }

        #voucher-link-2{
            font-size: 12px;
        }

        .head-content {
            margin-bottom: 2px;
        }

        .price-items li {
            margin-bottom: 5px;
        }

        .price-text {
            font-size: 14px;
        }

        .head-content input, .head-content button{
            font-size: 12px;
        }

        .customer-info p{
            font-size: 13px;
        }

        .address {
            margin-top:4px;
        }

        .change-address {
            font-size: 14px;
        }
    }

    @media (max-width:408px){
        .block-checkout-payment .btn-danger{
            font-size: 13px;
        }

        .block-checkout-payment .btn-primary {
            font-size: 14px;
            width:100%;
        }

        .price-text span {
            font-size: 12px;
        }

        #price-total-value-2 {
            font-size: 20px;
        }
    }
</style>
