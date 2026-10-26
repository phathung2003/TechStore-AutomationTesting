<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use App\Models\Product;
use Illuminate\Http\Request;
class ProductController extends Controller
{
    //
    public function getList(){
        $title = 'List of Products';
        $list = Product::all();
        if(!empty($list)) {
            return view('admin.product.list', compact('title','list'));
        }
        return view('admin.product.list',compact('title'));
    }
    public function getAdd() {
        $title = 'Add a new product';
        return view('admin.product.add',compact('title'));
    }
    public function postAdd(Request $req){
        $req ->validate([
            'code' => 'required | unique:products',
            'name' => 'required',
            'image' => 'required|image|mimes:jpeg,png,jpg,webp',
            'category' => 'required | regex:/^(?!0$).*/',
            'brand' => 'required | regex:/^(?!0$).*/',
            'amount' => 'required | integer | min:1',
            'storage' => 'required | integer | min:0',
            'color' => 'required',
            'price_buy' => 'required | integer | min:100',
            'price_sell' => 'required | integer | min:100',
        ]);
        $imageName = $req->file('image')->getClientOriginalName();
        $path = $req->file('image')->storeAs('images',$imageName,'public');
        $pathImage = '/storage'.'/'.$path;
        $product = new Product;
        $product->code = $req->code;
        $product->name = $req->name;
        $product->images = $pathImage;
        $product->category_id = $req->category;
        $product->brand_id = $req->brand;
        $product->amount = $req->amount;
        $product->storage = $req->storage;
        $product->color = $req->color;
        $product->price_buy = $req->price_buy;
        $product->price_sell = $req->price_sell;
        $check = $product->save();
        if($check) {
            return redirect()->route('admin.products.getList',compact('check'));
        }
        return redirect()->route('admin.products.getList');
    }

    public function getUpdate($id =0) {
        if(!empty($id) && ctype_digit($id)){
            $product = Product::find($id);
            if(!empty($product)) {
                return view('admin.product.update', compact('product'));
            }
        }
        return view('admin.product.update');
    }
    public function postUpdate(Request $req, $id){
    if(!empty($id) && ctype_digit($id)){
        $req ->validate([
            'code' => 'required | unique:products,code,'.$id,
            'name' => 'required',
            'image' => 'required|image|mimes:jpeg,png,jpg,webp',
            'category' => 'required | regex:/^(?!0$).*/',
            'brand' => 'required | regex:/^(?!0$).*/',
            'amount' => 'required | integer | min:1',
            'storage' => 'required | integer | min:0',
            'color' => 'required',
            'price_buy' => 'required | integer | min:100',
            'price_sell' => 'required | integer | min:100',
        ]);
        $imageName = $req->file('image')->getClientOriginalName();
        $path = $req->file('image')->storeAs('images',$imageName,'public');
        $pathImage = '/storage'.'/'.$path;
        $product = Product::find($id);
        $product->code = $req->code;
        $product->name = $req->name;
        $product->images = $pathImage;
        $product->category_id = $req->category;
        $product->brand_id = $req->brand;
        $product->amount = $req->amount;
        $product->storage = $req->storage;
        $product->color = $req->color;
        $product->price_buy = $req->price_buy;
        $product->price_sell = $req->price_sell;
        $check = $product->save();
        if($check) {
            return redirect()->route('admin.products.getList',compact('check'));
        }
    }
        return redirect()->route('admin.products.getList');
    }
    public function deleteItem($id=0){
        if(!empty($id) && ctype_digit($id)){
            $product = Product::find($id);
            if(!empty($product)) {
                $check = $product->delete();
                if($check) {
                    return redirect()->route('admin.products.getList',compact('check'));
                }
            }
        }
        return redirect()->route('admin.products.getList');
    }
}
