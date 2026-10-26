<?php

namespace App\Http\Controllers\Client;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Product;
use App\Models\Cart;
use App\Models\Order;
use App\Models\OrderDetail;
use App\Models\Rank;
use App\Models\Discount;
use App\Models\User;
use Auth;
use Session;
class CartController extends Controller
{
    //
    private $order;
    private $orderdetail;
    private $user;
    public function __construct(){
        $this->order = new Order();
        $this->orderdetail = new OrderDetail();
        $this->rank = new Rank();
        $this->user = new User();
        $this->discount = new Discount();
    }
    public function showCart(Request $req){
        if($req->input('vnp_SecureHash')) {
            $name = Session('Cart')->nickName ?? Auth::user()->nickName;
            $phone = Session('Cart')->phone ?? Auth::user()->phone;
            $address = Session('Cart')->address ?? Auth::user()->address;
            $order = new Order;
            $order->user_id = Auth()->user()->id;
            $order->name = $name;
            $order->total = 0;
            $order->status = 1;
            $order->address = $address;
            $order->phone = $phone;
            $order->save();
            $idOrder = $order->id;
            if(!empty(Session("Cart")->products)){
                foreach(Session("Cart")->products as $item){
                    $data = [
                        'order_id' => $idOrder,
                        'product_id' => $item['productInfo']->id,
                        'price' => $item['price'],
			'amount' => $item['quantity'],
			'created_at' =>date('Y-m-d H:i:s')
                    ];
                    $this->orderdetail->addOrderDetail($data);
                    $product = Product::find($item['productInfo']->id);
                    $product->amount = $product->amount - $item['quantity'];
                    $product->save();
                }
                $data = [
                    Session('Cart')->totalPrice
                ];

                $idRank = Auth()->user()->rank_id;
                $rank = Rank::find($idRank);
                $discount_rank = $rank->discount;
                if(!empty(Session::get('discount'))){
                    foreach(Session::get('discount') as $key => $item){
                        $total = Session('Cart')->totalPrice - $discount_rank*0.01*Session('Cart')->totalPrice;
                        $total = $total - $item['price'];
                        $data = [
                            $total
                        ];
                        $discount = Discount::find($item['id']);
                        $discount->amount = $discount->amount - 1;
                        $discount->save();
                        $discount = [
                            $item['id']
                        ];
                    }
                }

                $this->order->updatePriceOrder($idOrder,$data);
                if(!empty($discount))
                {
                    $this->order->updateDiscount($idOrder,$discount);
                }

                $user = User::find(Auth()->user()->id);
                $user->rank_id = $this->rank->setupRank($user->id) ;
                $user->save();
                $oldCart = Session('Cart') ? Session('Cart'):null;
                // $newCart = new Cart($oldCart);
                $oldCart->deleteAllCart();
                $req->session()->forget('Cart');
                $req->session()->forget('discount');
                return view('client.cart.index');
            }else{
                return view('client.cart.index');
            }
        };

        $idRank = Auth()->user()->rank_id;
        if(!empty($idRank)){
           $rank = Rank::find($idRank);
           $nameRank = $rank->name;
           $discount = $rank->discount;
        }
        return view('client.cart.index',compact('nameRank','discount'));
    }
    public function checkout(Request $req){
        $check = (Session('Cart')->nickName && Session('Cart')->phone && Session('Cart')->address) || (Auth::user()->phone && Auth::user()->address) ;
        if(!$check) {
            return response()->json([
                "status" => "error",
                "des" => "You dont have fill out your information to checkout"
            ]);
        } else {
            $name = Session('Cart')->nickName ?? Auth::user()->nickName;
            $phone = Session('Cart')->phone ?? Auth::user()->phone;
            $address = Session('Cart')->address ?? Auth::user()->address;
            $order = new Order;
            $order->user_id = Auth()->user()->id;
            $order->name = $name;
            $order->total = 0;
            $order->status = 1;
            $order->address = $address;
            $order->phone = $phone;
            $order->save();
            $idOrder = $order->id;
            if(!empty(Session("Cart")->products)){
                foreach(Session("Cart")->products as $item){
                    $data = [
                        'order_id' => $idOrder,
                        'product_id' => $item['productInfo']->id,
                        'price' => $item['price'],
                        'amount' => $item['quantity'],
                        'created_at' =>date('Y-m-d H:i:s')
                    ];
                    $this->orderdetail->addOrderDetail($data);
                    $product = Product::find($item['productInfo']->id);
                    $product->amount = $product->amount - $item['quantity'];
                    $product->save();
                }
                $data = [
                    Session('Cart')->totalPrice
                ];
                $idRank = Auth()->user()->rank_id;
                $rank = Rank::find($idRank);
                $discount_rank = $rank->discount;
                if(!empty(Session::get('discount'))){
                    foreach(Session::get('discount') as $key => $item){
                        Session('Cart')->totalPrice = Session('Cart')->totalPrice - (Session('Cart')->totalPrice * $discount_rank)/100;
                        Session('Cart')->totalPrice = Session('Cart')->totalPrice - $item['price'];
                        $data = [
                            Session('Cart')->totalPrice
                        ];
                        $discount = Discount::find($item['id']);
                        $discount->amount = $discount->amount - 1;
                        $discount->save();
                        $discount = [
                            $item['id']
                        ];
                    }
                }
                else{
                    // Session('Cart')->totalPrice = Session('Cart')->totalPrice - $discount_rank;
                    $data = [
                        Session('Cart')->totalPrice = Session('Cart')->totalPrice - (Session('Cart')->totalPrice * $discount_rank)/100
                    ];
                }
                $this->order->updatePriceOrder($idOrder,$data);
                if(!empty($discount))
                {
                    $this->order->updateDiscount($idOrder,$discount);
                }

                $user = User::find(Auth()->user()->id);
                $user->rank_id = $this->rank->setupRank($user->id) ;
                $user->save();
                $oldCart = Session('Cart') ? Session('Cart'):null;
                // $newCart = new Cart($oldCart);
                $oldCart->deleteAllCart();
                $req->session()->forget('Cart');
                $req->session()->forget('discount');
                return response([
                    "status" => "success"
                ]);
            }else{
                return response([
                    "status" => "cant"
                ]);
            }
        }
    }
    public function addToCart(Request $req, $id){
        $product = Product::find($id);
        if($product != null){
            $oldCart = Session('Cart') ? Session('Cart') : null;
            $newCart = new Cart($oldCart);
            $newCart->addToCart($product,$id);
            $req->session()->put('Cart',$newCart);
        }
        $amount = Session('Cart')->totalQuantity;
        return $amount;
    }
    public function updateItemListCart(Request $request, $id, $quantity) {
        $oldCart = Session('Cart') ? Session('Cart') : null;
        $newCart = new Cart($oldCart);
        $newCart->updateItemCart($id, $quantity);
        $request->session()->put('Cart',$newCart);
        if(!empty(Auth()->user()->rank_id)){
            $idRank = Auth()->user()->rank_id;
            $rank = Rank::find($idRank);
            $discount_rank = $rank->discount;
            $totalPrice = Session('Cart')->totalPrice - (Session('Cart')->totalPrice * $discount_rank)/100;
            $discountRank = (Session('Cart')->totalPrice * $discount_rank)/100;
            if(Session::get('discount')){
                foreach(Session::get('discount') as $key=>$item)
                {
                    $voucher = $item['price'];
                    return response([
                        Session('Cart')->products[$id],
                        Session('Cart')->totalQuantity,
                        Session('Cart')->totalPrice,
                        $totalPrice - $voucher,
                        $discountRank
                    ]);
                }

            }else{
                return response([
                    Session('Cart')->products[$id],
                    Session('Cart')->totalQuantity,
                    Session('Cart')->totalPrice,
                    $totalPrice,
                    $discountRank
                ]);
            }
        }else{
            return response([
                Session('Cart')->products[$id],
                Session('Cart')->totalQuantity,
                Session('Cart')->totalPrice,
                Session('Cart')->totalPrice,
                0
            ]);
        }
    }
    public function deleteItemListCart(Request $req, $id){
        $oldCart = Session('Cart') ? Session('Cart') : null;
        $newCart = new Cart($oldCart);
        $newCart->deleteItemCart($id);
        if(Count($newCart->products) > 0){
            $req->session()->put('Cart',$newCart);
            return response([
                Session('Cart')->totalQuantity,
                Session('Cart')->totalPrice
            ]);
        }
        else {
            $req->session()->forget('Cart');
            return response(0);
        }

    }
    public function getChangeInfoOrder(){
        return view('client.cart.change_infor_order');
    }
    public function postChangeInfoOrder(Request $req){
        $req->validate([
            "nickName" => 'required',
            "phone" => 'required',
            "address" => 'required'
        ]);
        $oldCart = Session('Cart') ? Session('Cart') : null;
        $newCart = new Cart($oldCart);
        $newCart->updateInfo($req->nickName, $req->phone, $req->address);
        $req->session()->put('Cart',$newCart);
        return redirect()->route('cart');
    }
    public function checkDiscount(Request $request){
        $code = $request->voucher;
        $discount = Discount::where('code',$code)->first();
        if($discount){
            $rankDiscount = $discount->rank_id;
            $rankCus = Auth()->user()->rank_id;
            if($rankDiscount == $rankCus){
                $count_discount = $discount->count();
                if($count_discount>0){
                    $discount_session = Session::get('discount');
                    if($discount_session==true){
                        $is_avaiable = 0;
                        if($is_avaiable==0){
                            $data[] = [
                                'id' => $discount->id,
                                'rank_id' => $discount->rank_id,
                                'code' => $discount->code,
                                'price' => $discount->price,
                                'name' => $discount->name
                            ];
                            Session::put('discount', $data);
                        }
                    }else{
                        $data[] = [
                            'id' => $discount->id,
                            'rank_id' => $discount->rank_id,
                            'code' => $discount->code,
                            'price' => $discount->price,
                            'name' => $discount->name
                        ];
                        Session::put('discount', $data);
                    }
                    Session::save();
                    return redirect()->back();
                }
            }else{
                return redirect()->back();
            }

        }else{
            return redirect()->back();
       }
    }

    public function checkDiscountSelect(Request $request){
        $code = $request->voucher;
        $discount = Discount::where('code',$code)->first();
        if($discount){
            $rankDiscount = $discount->rank_id;
            $rankCus = Auth()->user()->rank_id;
            if($rankDiscount == $rankCus){
                $count_discount = $discount->count();
                if($count_discount>0){
                    $discount_session = Session::get('discount');
                    if($discount_session==true){
                        $is_avaiable = 0;
                        if($is_avaiable==0){
                            $data[] = [
                                'id' => $discount->id,
                                'rank_id' => $discount->rank_id,
                                'code' => $discount->code,
                                'price' => $discount->price,
                                'name' => $discount->name
                            ];
                            Session::put('discount', $data);
                        }
                    }else{
                        $data[] = [
                            'id' => $discount->id,
                            'rank_id' => $discount->rank_id,
                            'code' => $discount->code,
                            'price' => $discount->price,
                            'name' => $discount->name
                        ];
                        Session::put('discount', $data);
                    }
                    Session::save();
                    //return view('client.cart.index');
                    return redirect()->route('cart');
                }
            }else{
                //return view('client.cart.index');
                return redirect()->route('cart');
            }

        }else{
            //return view('client.cart.index');
            return redirect()->route('cart');
       }
    }


    public function checkPayment(Request $req){
        // $check = (Session('Cart')->nickName && Session('Cart')->phone && Session('Cart')->address) || (Auth::user()->phone && Auth::user()->address) ;
        // if(!$check) {
        //     return response()->json([
        //         "status" => "error",
        //         "des" => "You dont have fill out your information to checkout"
        //     ]);
        // } else {
            $vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            $vnp_Returnurl = "http://sonhoang2071.site/cart";
            $vnp_TmnCode = "U2YDTOHX";//Mã website tại VNPAY
            $vnp_HashSecret = "KZGNSWGMVOOUJLTTKOAPYVBUCVHUXLQT"; //Chuỗi bí mật

            $vnp_TxnRef = rand(0, 9999); //Mã đơn hàng. Trong thực tế Merchant cần insert đơn hàng vào DB và gửi mã này sang VNPAY
            $vnp_OrderInfo = "Checkout Payment";
            $vnp_OrderType = "Techstore";
            $vnp_Amount = $req->totalPrice * 100 * 22000;
            $vnp_Locale = "VN";
            $vnp_BankCode = "NCB";
            $vnp_IpAddr = $_SERVER['REMOTE_ADDR'];

            $inputData = array(
                "vnp_Version" => "2.1.0",
                "vnp_TmnCode" => $vnp_TmnCode,
                "vnp_Amount" => $vnp_Amount,
                "vnp_Command" => "pay",
                "vnp_CreateDate" => date('YmdHis'),
                "vnp_CurrCode" => "VND",
                "vnp_IpAddr" => $vnp_IpAddr,
                "vnp_Locale" => $vnp_Locale,
                "vnp_OrderInfo" => $vnp_OrderInfo,
                "vnp_OrderType" => $vnp_OrderType,
                "vnp_ReturnUrl" => $vnp_Returnurl,
                "vnp_TxnRef" => $vnp_TxnRef
            );

            if (isset($vnp_BankCode) && $vnp_BankCode != "") {
                $inputData['vnp_BankCode'] = $vnp_BankCode;
            }
            if (isset($vnp_Bill_State) && $vnp_Bill_State != "") {
                $inputData['vnp_Bill_State'] = $vnp_Bill_State;
            }

            //var_dump($inputData);
            ksort($inputData);
            $query = "";
            $i = 0;
            $hashdata = "";
            foreach ($inputData as $key => $value) {
                if ($i == 1) {
                    $hashdata .= '&' . urlencode($key) . "=" . urlencode($value);
                } else {
                    $hashdata .= urlencode($key) . "=" . urlencode($value);
                    $i = 1;
                }
                $query .= urlencode($key) . "=" . urlencode($value) . '&';
            }

            $vnp_Url = $vnp_Url . "?" . $query;
            if (isset($vnp_HashSecret)) {
                $vnpSecureHash =   hash_hmac('sha512', $hashdata, $vnp_HashSecret);//
                $vnp_Url .= 'vnp_SecureHash=' . $vnpSecureHash;
            }
            $returnData = array('code' => '00'
                , 'message' => 'success'
                , 'data' => $vnp_Url);
                if (isset($_POST['redirect'])) {
                    header('Location: ' . $vnp_Url);
                    die();
                } else {
                    echo json_encode($returnData);
                }
        // }


    }
}
