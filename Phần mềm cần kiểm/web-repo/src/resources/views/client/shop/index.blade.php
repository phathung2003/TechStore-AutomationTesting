@extends('client.layouts.master')
@section('content')
    <style>
        :root {
	        --primary-color: #4daf54;
        }

        * {
            padding: 0;
            margin: 0;
            box-sizing: border-box;
        }
        .card {
            margin: 40px auto;
            height: 400px;
            background:#fff;
            padding: 40px 20px;
            color: #000;
            position: relative;
            border-radius:6px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
        }

        .card__img {
            width: 100%;
            transition: 0.5s;
            display: flex;
            justify-content: center;
        }

        .card:hover .card__img {
            transform: translateY(-90px);
        }

        .card__title {
            color: black;
            transition: 0.5s;
            text-align: center;
            font-size: 20px;
        }

        .card__price {
            font-weight: 600;
            font-size: 28px;
            transition: 0.5s;
            text-align: center;
            margin: 8px 0 0px;
            color: black;
        }

        .card:hover .card__title {
            transform: translate(-32px, -72px);
        }

        .card:hover .card__price {
            transform: translate(-76px, -60px);
        }
        .card:hover {
            cursor: pointer;
        }
        .card__size,
        .card__color,
        .card__action {
            opacity: 0;
            visibility: hidden;
            transition: 0.5s;
            font-size: 16px;
        }

        .card__size {
            margin-top: 100px;
        }

        .card:hover .card__size {
            margin-top: -30px;
        }

        .card:hover .card__size,
        .card:hover .card__color,
        .card:hover .card__action {
            transition-delay: 0.1s;
            opacity: 1;
            visibility: visible;
        }

        .card__size,
        .card__color {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
        }

        .card__size h3,
        .card__color h3 {
            font-weight: unset;
            margin-right: 12px;
        }

        .card__size span {
            padding: 2px 10px;
            background-color: #dadada;
            margin: 0 5px;
            border-radius: 5px;
            color: #272d40;
            cursor: pointer;
        }

        .card__color span {
            width: 20px;
            height: 20px;
            border-radius: 50%;
            margin: 0 5px;
            cursor: pointer;
        }


        .card__action button {
            padding: 10px 16px;
            outline: none;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            color: white;
            margin-right: 7px;
            font-weight: 600;
            transition: 0.25s;
        }

        .card__action button:hover {
            transform: scale(0.9);
        }


        .like, .cart {
            font-size: 25px;
            position: absolute;
            top: 12px;
            left: 20px;
            cursor: pointer;
            z-index: 2;
        }
        .card__color span.green {
	        background-color: green;
        }
        .card__color span.red {
            background-color: red;
        }
        .card__color span.black {
            background-color: black;
        }

        .cart {
            left: unset;
            right: 20px;
        }
        .buy-now {
            background-color: #f52f32;
        }
        .add-cart {
            background-color: var(--primary-color);
        }
        .filter {
            background: #fff;
            border: 1px solid #eee;
            margin-top: 40px;
            width: 30%;
            max-height: 1000px;
        }

        .product_item{
            width: 24%;
            margin: 5px;
        }
        @media (max-width:1200px){
            .card__action button{
                margin-right:0;
                width: 50%;
                font-size: 12px;
            }
            .card__action a{
                font-size: 13px;
            }

            .card:hover .card__price {
                transform: translate(-50px, -60px);
            }

            .product_item{
                width: 24%;
                margin: 3px;
            }
        }

        @media (max-width:990px){
            .product_item{
                width: 30%;
            }

            .card__action button{
                margin-right:0;
                width: 85px;
                font-size: 12px;
            }
        }

        @media (max-width:770px){
            .shell {
                margin-left: 40px;
            }
            .product_item{
                width: 45%;
            }
        }

        @media (max-width:471px){
            .shell {
                margin-left: 0;
            }
            .row {
                justify-content: center;
            }
            .product_item{
                width: 55%;
            }
        }

        @media (max-width:395px){
            .product_item{
                width: 65%;
            }
        }
    </style>
    <div class="container" style="max-width: 1270px; margin-top: 16px;">
            {{-- <div class="filter">
            </div> --}}
            <div class="shell" >
                <div class="container">
                    <div class="row">
                        @if(!empty($data))
                            @foreach($data as $item)
                                <div class="product_item">
                                    <a href="{{route('getDetailProduct',[ 'id' => $item->id ] )}}">
                                        <div class="card">
                                            {{-- <span class="like"><i class='bx bx-heart'></i></span>
                                            <span class="cart"><i class='bx bx-cart-alt' ></i></span> --}}
                                            <div class="card__img">
                                                <img src="{{asset($item->images)}}" alt="" style="width:90%"/>
                                            </div>
                                            <h2 class="card__title">{{$item->name}}</h2>
                                            <p class="card__price">{{$item->price_sell}}$</p>
                                            <div class="card__size">
                                                <h3 style="font-size: 16px; margin : 0 12px 0 0">Storage:</h3>
                                                <span>{{$item->storage}}</span>
                                            </div>
                                            <div class="card__color">
                                                <h3 style="font-size: 16px;margin : 0 12px 0 0">Color:</h3>
                                                <span style="background-color: {{$item->color}}"></span>
                                            </div>
                                            <div class="card__action">
                                                <button class="buy-now">Buy now</button>
                                                <a onclick="addToCart({{$item ->id}})" href="javascript:" class="add-cart btn" style="color:#fff">Add cart</a>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            @endforeach
                        @elseif(!empty($category))
                            @foreach($category as $key)
                                @foreach(getProduct($key->id) as $item)
                                    <div class="product_item">
                                        <a href="{{route('getDetailProduct',[ 'id' => $item->id ] )}}">
                                            <div class="card">
                                                {{-- <span class="like"><i class='bx bx-heart'></i></span>
                                                <span class="cart"><i class='bx bx-cart-alt' ></i></span> --}}
                                                <div class="card__img">
                                                    <img src="{{asset($item->images)}}" alt="" style="width:90%"/>
                                                </div>
                                                <h2 class="card__title">{{$item->name}}</h2>
                                                <p class="card__price">{{$item->price_sell}}$</p>
                                                <div class="card__size">
                                                    <h3 style="font-size: 16px; margin : 0 12px 0 0">Storage:</h3>
                                                    <span>{{$item->storage}}</span>
                                                </div>
                                                <div class="card__color">
                                                    <h3 style="font-size: 16px;margin : 0 12px 0 0">Color:</h3>
                                                    <span style="background-color: {{$item->color}}"></span>
                                                </div>
                                                <div class="card__action">
                                                    <button class="buy-now">Buy now</button>
                                                    <a onclick="addToCart($item->id}})" href="javascript:" class="add-cart btn" style="color:#fff">Add cart</a>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                @endforeach
                            @endforeach
                        @else
                                @if (!empty($list))
                                    @foreach ($list as $item)
                                        <div class="product_item">
                                            <a href="{{route('getDetailProduct',[ 'id' => $item->id ] )}}">
                                                <div class="card">
                                                    {{-- <span class="like"><i class='bx bx-heart'></i></span>
                                                    <span class="cart"><i class='bx bx-cart-alt' ></i></span> --}}
                                                    <div class="card__img">
                                                        <img src="{{asset($item->images)}}" alt="" style="width:90%"/>
                                                    </div>
                                                    <h2 class="card__title">{{$item->name}}</h2>
                                                    <p class="card__price">{{$item->price_sell}}$</p>
                                                    <div class="card__size">
                                                        <h3 style="font-size: 16px; margin : 0 12px 0 0">Storage:</h3>
                                                        <span>{{$item->storage}}</span>
                                                    </div>
                                                    <div class="card__color">
                                                        <h3 style="font-size: 16px;margin : 0 12px 0 0">Color:</h3>
                                                        <span style="background-color: {{$item->color}}"></span>
                                                    </div>
                                                    <div class="card__action">
                                                        <button class="buy-now">Buy now</button>
                                                        <a onclick="addToCart({{$item ->id}})" href="javascript:" class="add-cart btn" style="color:#fff">Add cart</a>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                    @endforeach
                                @else
                                        <h2>Data not available</h2>
                                @endif
                        @endif
                    </div>
		</div>
		<div style="margin-top:24px">
                    {{$list->links("pagination::bootstrap-5")}}
                </div>
            </div>
    </div>
    <script>
        function addToCart(id){
            $.ajax({
                url:'addToCart/'+id,
                type:'GET',
            }).done(function(respone){
                $("#icon-amount-orders").text(`${respone}`);
                Swal.fire(
                    'Add to cart successfully',
                    'You clicked the button!',
                    'success'
                )
            });
        }
    </script>

@endsection
