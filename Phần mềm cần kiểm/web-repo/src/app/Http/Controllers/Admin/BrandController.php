<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Brand;
class BrandController extends Controller
{

    public function getList(){
        $title = 'List of Brands';
        $list = Brand::all();
        if(!empty($list)) {
            return view('admin.brand.list', compact('title','list'));
        }
        return view('admin.brand.list',compact('title'));
    }
    public function getAdd(){
        $title = 'Add a new brand';
        return view('admin.brand.add', compact('title'));
    }
    public function postAdd(Request $req){
        $req ->validate([
            'name' => 'required | unique:brands',
            'description' => 'required | string'
        ]);
        $brand = new Brand;
        $brand->name = $req->name;
        $brand->description = $req->description;
        $check = $brand->save();
        if($check) {
            return redirect()->route('admin.brands.getList',compact('check'));
        }
        return redirect()->route('admin.brands.getList');

    }
    public function getUpdate($id = 0){
        if(!empty($id) && ctype_digit($id)){
            $brand = Brand::find($id);
            if(!empty($brand)) {
                return view('admin.brand.update', compact('brand'));
            }
        }
        return view('admin.brand.update');
    }
    public function postUpdate(Request $req, $id){
        if(!empty($id) && ctype_digit($id)){
            $req ->validate([
                'name' => 'required | unique:brands,name,'.$id,
                'description' => 'required | string',
            ]);
            $brand = Brand::find($id);
            if(!empty($brand)) {
                $brand->name = $req->name;
                $brand->description = $req->description;
                $check = $brand->save();
                if($check) {
                    return redirect()->route('admin.brands.getList',compact('check'));
                }
            }
        }
        return redirect()->route('admin.brands.getList');
    }
    public function deleteItem($id = 0){
        if(!empty($id) && ctype_digit($id)){
            $brand = Brand::find($id);
            if(!empty($brand)) {
                $check = $brand->delete();
                if($check) {
                    return redirect()->route('admin.brands.getList',compact('check'));
                }
            }
        }
        return redirect()->route('admin.brands.getList');
    }
}
