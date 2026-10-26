@extends('client.layouts.master')
@section('content')
<style>
    body {
      font-family: Nunito;
      overflow-x: hidden; }

    img {
      max-width: 100%; }

    .preview {
      display: -webkit-box;
      display: -webkit-flex;
      display: -ms-flexbox;
      display: flex;
      -webkit-box-orient: vertical;
      -webkit-box-direction: normal;
      -webkit-flex-direction: column;
          -ms-flex-direction: column;
              flex-direction: column; }
      @media screen and (max-width: 996px) {
        .preview {
          margin-bottom: 20px; } }

    .preview-pic {
      -webkit-box-flex: 1;
      -webkit-flex-grow: 1;
          -ms-flex-positive: 1;
              flex-grow: 1; }

    .preview-thumbnail.nav-tabs {
      border: none;
      margin-top: 15px; }
      .preview-thumbnail.nav-tabs li {
        width: 18%;
        margin-right: 2.5%; }
        .preview-thumbnail.nav-tabs li img {
          max-width: 100%;
          display: block; }
        .preview-thumbnail.nav-tabs li a {
          padding: 0;
          margin: 0; }
        .preview-thumbnail.nav-tabs li:last-of-type {
          margin-right: 0; }

    .tab-content {
      overflow: hidden; }
      .tab-content img {
        width: 100%;
        -webkit-animation-name: opacity;
                animation-name: opacity;
        -webkit-animation-duration: .3s;
                animation-duration: .3s; }

    .card {
      margin-top: 50px;
      background: #eee;
      padding: 3em;
      line-height: 1.5em; }

    @media screen and (min-width: 997px) {
      .wrapper {
        display: -webkit-box;
        display: -webkit-flex;
        display: -ms-flexbox;
        display: flex; } }

    .details {
      display: -webkit-box;
      display: -webkit-flex;
      display: -ms-flexbox;
      display: flex;
      -webkit-box-orient: vertical;
      -webkit-box-direction: normal;
      -webkit-flex-direction: column;
          -ms-flex-direction: column;
              flex-direction: column; }

    .colors {
      -webkit-box-flex: 1;
      -webkit-flex-grow: 1;
          -ms-flex-positive: 1;
              flex-grow: 1; }

    .product-title, .price, .sizes, .colors {
      /* text-transform: UPPERCASE; */
      font-weight: bold; }

    .checked, .price span {
      color: #B82121; }

    .product-title, .rating, .product-description, .price, .vote, .sizes {
      margin-bottom: 15px; }

    .product-title {
      margin-top: 0; }

    .size {
      margin-right: 10px; }
      .size:first-of-type {
        margin-left: 40px; }

    .color {
      display: inline-block;
      vertical-align: middle;
      margin-right: 10px;
      height: 2em;
      width: 2em;
      border-radius: 2px; }
      .color:first-of-type {
        margin-left: 20px; }

    .add-to-cart, .like {
      background: #eee;
      color: black;
      padding: 1.2em 1.5em;
      border: 1px solid black;
      text-transform: UPPERCASE;
      font-weight: bold;
      -webkit-transition: background .3s ease;
              transition: background .3s ease; }
      .add-to-cart:hover, .like:hover {
        background: #B82121;
        color: #fff;
        border: none;
    }

    .not-available {
      text-align: center;
      line-height: 2em; }
      .not-available:before {
        font-family: Nunito;
        content: "\f00d";
        color: #fff; }

    .orange {
      background: #ff9f1a; }

    .green {
      background: #85ad00; }

    .blue {
      background: #0076ad; }

    .tooltip-inner {
      padding: 1.3em; }

    @-webkit-keyframes opacity {
      0% {
        opacity: 0;
        -webkit-transform: scale(3);
                transform: scale(3); }
      100% {
        opacity: 1;
        -webkit-transform: scale(1);
                transform: scale(1); } }

    @keyframes opacity {
      0% {
        opacity: 0;
        -webkit-transform: scale(3);
                transform: scale(3); }
      100% {
        opacity: 1;
        -webkit-transform: scale(1);
                transform: scale(1); } }

    /*# sourceMappingURL=style.css.map */
    /* css comment */
    body{

    background:#ebeef0;
}

.img-sm {
    width: 46px;
    height: 46px;
}

.panel {
    box-shadow: 0 2px 0 rgba(0,0,0,0.075);
    border-radius: 0;
    border: 0;
    margin-bottom: 15px;
}

.panel .panel-footer, .panel>:last-child {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
}

.panel .panel-heading, .panel>:first-child {
    border-top-left-radius: 0;
    border-top-right-radius: 0;
}

.panel-body {
    padding: 25px 20px;
}


.media-block .media-left {
    display: block;
    float: left
}

.media-block .media-right {
    float: right
}

.media-block .media-body {
    display: block;
    overflow: hidden;
    width: auto
}

.middle .media-left,
.middle .media-right,
.middle .media-body {
    vertical-align: middle
}

.thumbnail {
    border-radius: 0;
    border-color: #e9e9e9
}

.tag.tag-sm, .btn-group-sm>.tag {
    padding: 5px 10px;
}

.tag:not(.label) {
    background-color: #fff;
    padding: 6px 12px;
    border-radius: 2px;
    border: 1px solid #cdd6e1;
    font-size: 12px;
    line-height: 1.42857;
    vertical-align: middle;
    -webkit-transition: all .15s;
    transition: all .15s;
}
.text-muted, a.text-muted:hover, a.text-muted:focus {
    color: #acacac;
}
.text-sm {
    font-size: 0.9em;
}
.text-5x, .text-4x, .text-5x, .text-2x, .text-lg, .text-sm, .text-xs {
    line-height: 1.25;
}

.btn-trans {
    background-color: transparent;
    border-color: transparent;
    color: #929292;
}

.btn-icon {
    padding-left: 9px;
    padding-right: 9px;
}

.btn-sm, .btn-group-sm>.btn, .btn-icon.btn-sm {
    padding: 5px 10px !important;
}

.mar-top {
    margin-top: 15px;
}
/* .fa:hover {
  color: darkblue;
}
.active {
  color: darkblue;
} */
.likeButton {
  color: #858796;
}
</style>
@if (!empty($product))
<div class="container">
        <div class="card">
            <div class="container-fliud">
                <div class="wrapper row">
                    <div class="preview col-md-6">
                        <div class="preview-pic tab-content">
                        <div class="tab-pane active" id="pic-1">
                            <img src="{{asset($product['images'])}}"/>
                        </div>
                        </div>
                    </div>
                    <div class="details col-md-6">
                        <h3 class="product-title">{{$product['name']}}</h3>
                        <div class="rating">
                            <div class="stars">
                                <span class="fa fa-star checked"></span>
                                <span class="fa fa-star checked"></span>
                                <span class="fa fa-star checked"></span>
                                <span class="fa fa-star"></span>
                                <span class="fa fa-star"></span>
                            </div>
                            <span class="review-no">41 reviews</span>
                        </div>
                        <p class="product-description">Suspendisse quos? Tempus cras iure temporibus? Eu laudantium cubilia sem sem! Repudiandae et! Massa senectus enim minim sociosqu delectus posuere.</p>
                        <h4 class="price">Current price: <span>{{$product['price_sell']}}$</span></h4>
                        <p class="vote"><strong>91%</strong> of buyers enjoyed this product! <strong>(87 votes)</strong></p>
                        <h5 class="sizes">Storage
                            <span class="size" data-toggle="tooltip" title="small">{{$product['storage']}}</span>
                        </h5>
                        <h5 class="colors">colors:
                            <span class="color" style="background-color: {{$product['color']}}"></span>
                        </h5>
                        <div class="action card__action">
                            <a onclick="addToCart({{$product['id']}})" href="javascript:" class="add-to-cart btn btn-default add-cart btn ">add to cart</a>
                            <a onclick="addToFavourite({{$product['id']}})" href="javascript:" class="like btn btn-default"><span class="fa fa-heart"></span></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</div>

<div class="container bootdey">
  <div class="col-md-12 bootstrap snippets">
    <div class="panel">
      <form action="{{route('getComment',['id'=> $product['id']])}}" method="POST">
        <div class="panel-body">
            <input name="content" class="form-control" rows="2" placeholder="What are you thinking?" autocomplete="off"/>
            <div class="mar-top clearfix">
              <button class="btn btn-sm btn-primary pull-right" type="submit"><i class="fa fa-pencil fa-fw"></i> Share</button>
            </div>
        </div>
        @csrf
      </form>
    </div>
    <div class="panel">
      @foreach($product->comments as $comment)
        <div class="panel-body">
            <div class="media-block">
              <a class="media-left" href="#"><img class="img-circle img-sm" alt="Profile Picture" src="https://bootdey.com/img/Content/avatar/avatar1.png"></a>
              <div class="media-body">
                <div class="mar-btm">
                  <a href="#" class="btn-link text-semibold media-heading box-inline ml-2">{{nameUser($comment->user_id)}}</a>
                  <p class="text-muted text-sm"><i class="fa fa-lg ml-2"></i>{{ $comment->created_at }}</p>
                </div>
                <p class="ml-2">{{ $comment->content }}</p>
                <div class="pad-ver">
                    <div class="btn-group">
                      <i id="likeButton" onclick="toggleLike()" class="btn btn-sm btn-default fa fa-thumbs-up"></i>
                      <i class="btn btn-sm btn-default fa fa-thumbs-down" style="color:#858796"></i>
                    </div>
                </div>
              <hr>
              </div>
            </div>
        </div>
      @endforeach
    </div>
</div>
  <script>
    function addToCart(id){
        $.ajax({
            url:'addCart/'+id,
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
    function addToFavourite(id){
        var $n = $("#icon-amount-favorite");
        $.ajax({
            url:'postFavorite/'+id,
            type:'GET',
        }).done(function(response){
          if(response.status == 'error')
          {
            Swal.fire(
                'Product is existed in your favorites',
                'You clicked the button!',
                'success'
            )
          }
          else {
            $n.text(`${response.data}`);
            Swal.fire(
                'Add to favorite successfully',
                'You clicked the button!',
                'success'
            )
          }

        });
    }
    function toggleLike(){
      const likeButton = document.getElementById('likeButton');
      if(likeButton.style.color = '#858796'){
          likeButton.style.color = '#3498db';
        } else {
          likeButton.style.color = '#858796';
        }
    }
  </script>
@endif
@endsection

@section('ext-script')

@endsection

