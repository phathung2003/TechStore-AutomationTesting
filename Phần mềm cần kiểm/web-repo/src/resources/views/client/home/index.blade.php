@extends('client.layouts.master')
@section('content')
    <div class="container" style="max-width: 55%" >
        <div class="splide splide_show">
            <div class="splide__track">
              <ul class="splide__list">
                <li class="splide__slide"><img style="height:360px"  src="{{asset('images/banner-1.webp')}}" alt=""></li>
                <li class="splide__slide"><img style="height:360px"  src="{{asset('images/15-pro.webp')}}" alt=""></li>
                <li class="splide__slide"><img style="height:360px" src="{{asset('images/samsung-galaxy.webp')}}" alt=""></li>
                <li class="splide__slide"><img style="height:360px" src="{{asset('images/sliding-oppo.webp')}}" alt=""></li>
                <li class="splide__slide"><img style="height:360px" src="{{asset('images/pova-5.webp')}}" alt=""></li>
                <li class="splide__slide"><img style="height:360px" src="{{asset('images/xiaomi-tv.webp')}}" alt=""></li>
              </ul>
            </div>

            <!-- Add the progress bar element -->
            <div class="my-slider-progress">
                <div class="my-slider-progress-bar"></div>
            </div>
        </div>
    </div>
    @if(!empty(categories()))
    @foreach(categories() as $item)
    <div class="text-center container py-2">
      <div class="d-flex justify-content-start" >
        <a href="{{route('getCategory',['data'=>"$item->name"])}}">
          <div class="d-flex justify-content-start" style="color: black" >
            <h4 class="mt-4 mb-5"><strong>{{$item->name}}</strong></h4>
          </div>
        </a>
      </div>
        <div class="row">
          @foreach(homeCateProduct($item->id) as $key)
            <div class="col-lg-2 col-md-12 mb-3 ml-3 mr-3" style="max_height:334.350;border-radius:10px 10px 0 0; box-shadow: 0px 0px 8px #c7bfbf">
              <div class="card">
                <a href="{{route('getDetailProduct',[ 'id' => $key->id ] )}}">
                  <div class="bg-image hover-zoom ripple ripple-surface ripple-surface-light"
                    data-mdb-ripple-color="light">
                    <img src="{{asset($key->images)}}"
                      class="w-100" />
                  </div>
                  <div class="card-body">
                      <h5 class="card-title mb-3" style="color:black">{{$key->name}}</h5>
                    <h6 class="mb-3" style="color:red">${{$key->price_sell}}</h6>
                  </div>
                  <div class="mask">
                    <div class="d-flex justify-content-center align-items-center">
                      <h5><span class="badge bg-primary ms-2" style="color: white">Buy now</span></h5>
                    </div>
                  </div>
                  <div class="hover-overlay">
                    <div class="mask" style="background-color: rgba(251, 251, 251, 0.15);"></div>
                  </div>
                </a>

              </div>
            </div>
            @endforeach
        </div>
    </div>
    @endforeach
    @endif
<script src="https://cdn.jsdelivr.net/npm/@splidejs/splide@4.1.4/dist/js/splide.min.js"></script>
<script>
  var splide_show = new Splide( '.splide_show', {
  type    : 'loop',
  perPage : 1,
  autoplay: true,
} );
splide_show.mount();
</script>
<style>
  .special{
    background: linear-gradient(rgb(138, 5, 5), rgb(0, 0, 0)) 0% 0% / cover;
    border-radius:16px;
    padding: 2px;
    margin:7px 0 15px;
    min-height:350px;

  }
    @media (max-width:700px) {
        img {
            width: 98%;
        }
    }
  .my-slider-progress {
    background: #ccc;
  }

  .my-slider-progress-bar {
    background: greenyellow;
    height: 2px;
    transition: width 400ms ease;
    width: 0;
  }
</style>
@endsection
