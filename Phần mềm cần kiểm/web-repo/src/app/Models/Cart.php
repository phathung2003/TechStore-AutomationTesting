<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Cart extends Model
{
    use HasFactory;
    public $products = null;
    public $totalPrice = 0;
    public $totalQuantity = 0;
    public $nickName = null;
    public $phone = null;
    public $address = null;
    public function __construct($cart = null) { //Mới sửa. Nguyên gốc: public function __construct($cart)
        if($cart){
            $this->products = $cart->products;
            $this->totalPrice = $cart->totalPrice;
            $this->totalQuantity = $cart->totalQuantity;
        }
    }
    public function addToCart($product, $id){
        $newProduct = ['quantity' => 0,'price' => $product->price_sell, 'productInfo' => $product];
        if($this->products){
            if(array_key_exists($id, $this->products)) {
                $newProduct = $this->products[$id];
            }
        }
        $newProduct['quantity']++;
        $newProduct['price'] = $newProduct['quantity'] * $product->price_sell;
        $this->products[$id] = $newProduct;
        $this->totalPrice += $product->price_sell;
        $this->totalQuantity++;
    }
    public function updateItemCart($id, $quantity) {
        $this->totalQuantity -= $this->products[$id]['quantity'];
        $this->totalPrice -= $this->products[$id]['price'];
        $this->products[$id]['quantity'] = $quantity;
        $this->products[$id]['price'] = $quantity * $this->products[$id]['productInfo']->price_sell;
        $this->totalQuantity += $this->products[$id]['quantity'];
        $this->totalPrice += $this->products[$id]['price'];
        $this->phone = Session('Cart')->phone;
        $this->address = Session('Cart')->address;
    }
    public function deleteItemCart($id){
        $this->totalPrice -= $this->products[$id]['price'];
        $this->totalQuantity -= $this->products[$id]['quantity'];
        unset($this->products[$id]);
    }
    public function deleteAllCart(){
        $this->products = [];
        $this->totalPrice = 0;
        $this->totalQuantity = 0;
        $this->phone = null;
        $this->address = null;
    }
    public function updateInfo($name, $phone, $address) {
        $this->nickName = $name;
        $this->phone = $phone;
        $this->address = $address;
    }
}
